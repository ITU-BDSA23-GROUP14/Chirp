using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Headers;

namespace Chirp.Tests;

public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public IntegrationTests(CustomWebApplicationFactory<Program> fixture)
    {
        // Arrange
        _fixture = fixture;
        
        // Change/remove authorisation to be able to test private timelines, code taken from https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
        _client = _fixture.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });
            });
        })
        .CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true,
            AllowAutoRedirect = false,
        });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "TestScheme");
    }

    [Fact]
    public async void CanSeePublicTimeline()
    {
        // Act
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var cheepText = "Hello, BDSA students!";

        // Assert
        Assert.Contains(cheepText, content);
    }

    [Theory]
    [InlineData("Helge", "Hello, BDSA students!")]
    [InlineData("Rasmus", "Hej, velkommen til kurset.")]
    public async void CanSeePrivateTimeline(string author, string expectedMessage)
    {
        // Act
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains(expectedMessage, content);
    }

    [Fact]
    public async void Page5000Loads()
    {
        // Act
        var response = await _client.GetAsync("/?page=5000");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async void Page0And1AreTheSame()
    {
        // Act
        var page0Response = await _client.GetAsync("/?page=0");
        var page1Response = await _client.GetAsync("/?page=1");

        page0Response.EnsureSuccessStatusCode();
        page1Response.EnsureSuccessStatusCode();
        var content1 = await page0Response.Content.ReadAsStringAsync();
        var content2 = await page1Response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains(content1, content2);
    }

    [Fact]
    public async void Page1And2AreDifferent()
    {
        // Act
        var page1Response = await _client.GetAsync("/?page=1");
        var page2Response = await _client.GetAsync("/?page=2");

        page1Response.EnsureSuccessStatusCode();
        page2Response.EnsureSuccessStatusCode();
        var content1 = await page1Response.Content.ReadAsStringAsync();
        var content2 = await page2Response.Content.ReadAsStringAsync();

        // Assert
        Assert.DoesNotContain(content1, content2);
    }

    [Fact]
    public async void Page1Contains32Cheeps()
    {
        // Act
        var pageResponse = await _client.GetAsync("/?page=1");

        pageResponse.EnsureSuccessStatusCode();
        var content = await pageResponse.Content.ReadAsStringAsync();

        // Adapted from https://stackoverflow.com/questions/541954/how-to-count-occurrences-of-a-char-string-within-a-string
        int count = content.Split("<li>").Length - 1;

        // Assert
        Assert.Equal(32, count);
    }
}

// Mock AuthHandler to be able to access private timelines taken from https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}