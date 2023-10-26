using Microsoft.AspNetCore.Mvc.Testing;
using Main;

namespace RazorTest
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> fixture)
        {
            // Arrange
            _fixture = fixture;
            _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
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
}