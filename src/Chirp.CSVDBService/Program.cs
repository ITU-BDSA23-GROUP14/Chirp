using CheepRecordType;
using SimpleDB;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var db = CSVDatabase<Cheep>.Instance("../../data/chirps.csv");

// This is your Web API
app.MapGet("/cheeps", () => db.Read());
app.MapPost("/cheep", (Cheep cheep) => { db.Store(cheep); });

// Create an HTTP client object
var baseURL = "http://localhost:5012";
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);

app.Run();