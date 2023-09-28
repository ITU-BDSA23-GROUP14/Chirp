namespace RemoteDatabase
{
    using DatabaseInterface;
    using System;
    using System.Net.Http.Json;

    public sealed class RemoteDatabase<T> : IDatabaseRepository<T>
    {
        private static RemoteDatabase<T>? instance = null; 
        private readonly HttpClient client;

        private RemoteDatabase(string filePath)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(filePath)
            };
        }

        public static RemoteDatabase<T> Instance(string filePath)
        { 
            if (instance == null)
            {
                instance = new RemoteDatabase<T>(filePath);
            }
            return instance;
        }

        public IEnumerable<T> Read(int? limit = null)
        {
            var cheep = client.GetFromJsonAsync<IEnumerable<T>>("/cheeps").Result;
            
            if (cheep != null) {
                return cheep;
            } else {
                return new List<T>();
            }
        }

        public void Store(T record)
        {
            var HTTPResponse = client.PostAsJsonAsync("/cheep", record);

            var result = HTTPResponse.Result;

            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine("Cheep successfully posted.");
            }
            else
            {
                Console.WriteLine($"Failed to post cheep. Status code: {HTTPResponse.Status} ");
            }
        }
    }
}