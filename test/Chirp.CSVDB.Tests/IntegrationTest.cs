/* using System.Net.Http.Json;
using SimpleDB;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Chirp.CSVDB.Tests
{
    public class IntegrationTest //: IDisposable
    {
        [Fact]
        public async void ReadTestRemote()
        {
            var baseURL = "https://bdsagroup14chirpremotedb.azurewebsites.net/cheeps";
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
            Assert.Equal(new Cheep(Author: "ropf", Message: "Hello, BDSA students!", Timestamp: 1690891760), responseContent.First());
        }

        [Fact]
        public async void WriteTestRemote()
        {
            var baseURL = "https://bdsagroup14chirpremotedb.azurewebsites.net/cheep";
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);

            Cheep? cheep = new("tester", "test cheep", 0);

            var HTTPResponse = await client.PostAsJsonAsync("cheep", cheep);
            var responseCode = HTTPResponse.IsSuccessStatusCode;

            Assert.True(responseCode);
        }

        [Fact]
        public void ReadWriteTest()
        {
            string path = @"chirps.csv";
            IDatabaseRepository<Cheep> csvh = CSVDatabase<Cheep>.Instance(path);
            List<Cheep> temp = csvh.Read().ToList();

            Cheep newCheep = new Cheep("me", "hello", 10);
            csvh.Store(newCheep);
            List<Cheep> temp2 = csvh.Read().ToList();

            for (int i = 0; i < temp2.Count; i++)
            {
                if (i < temp2.Count - 1)
                {
                    Assert.True(temp[i] == temp2[i]);
                }
                else
                {
                    Assert.True(temp2[i] == newCheep);
                    break;
                }
            }
        }
    }
} */