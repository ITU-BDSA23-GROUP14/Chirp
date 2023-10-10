using Microsoft.AspNetCore.Mvc.Testing;
using Main;

namespace RazorTest
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;
        private readonly HttpClient _client;

        public IntegrationTest(WebApplicationFactory<Program> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
        }

        [Fact]
        public async void CanSeePublicTimeline()
        {
            var response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var cheepText = "Hello, BDSA students!";  // This is the earliest registered cheep (ie. it shows up on the first page and is thus included in the "/" response)

            Assert.Contains(cheepText, content);
        }

        [Theory]
        [InlineData("Helge", "Hello, BDSA students!")]
        [InlineData("Rasmus", "Hej, velkommen til kurset.")]
        public async void CanSeePrivateTimeline(string author, string expectedMessage)
        {
            var response = await _client.GetAsync($"/{author}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains(expectedMessage, content);
        }
    }
}