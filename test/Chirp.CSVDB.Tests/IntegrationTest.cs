using System.Net.Http.Json;
using SimpleDB;
using System.Net.Http.Headers;

namespace Chirp.CSVDB.Tests
{
    public class IntegrationTest //: IDisposable
    {
        [Fact]
        public async void ReadTest()
        {

            var baseURL = "http://localhost:5000";
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);

            var HTTPResponse = await client.GetAsync("cheeps");
            var responseContent = await HTTPResponse.Content.ReadFromJsonAsync<IEnumerable<Cheep>>();
            var responseCode = HTTPResponse.StatusCode;

            Assert.Equal(System.Net.HttpStatusCode.OK, responseCode);
            Assert.NotNull(responseContent);
            Assert.Contains(responseContent, item => item is not null);
        }

        [Fact]
        public async void WriteTest()
        {
            var baseURL = "http://localhost:5000";
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);

            Cheep? cheep = new("tester", "test cheep", 0);
            
            var HTTPResponse = await client.PostAsJsonAsync("cheep", cheep);
            var responseCode = HTTPResponse.IsSuccessStatusCode;

            Assert.Equal(true, responseCode);
        }

        record Cheep(string Author, string Message, long Timestamp);
    }
}