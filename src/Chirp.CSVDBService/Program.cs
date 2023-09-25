using CheepRecordType;
using SimpleDB;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var db = CSVDatabase<Cheep>.Instance("chirps.csv");

// This is your Web API
app.MapGet("/cheeps", () => db.Read());
app.MapPost("/cheep", (Cheep cheep) => { db.Store(cheep); });

app.Run();

/*
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
*/
