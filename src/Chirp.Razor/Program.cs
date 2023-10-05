using Chirp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? chirpDbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");       // Attempt to get the database path from the environment variable

// If succesfullly located then use the path, otherwise create it
if (string.IsNullOrEmpty(chirpDbPath))
{
    //sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db");
    chirpDbPath = "data/chirp.db";
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite($"Data Source={chirpDbPath}"));
builder.Services.AddTransient<ICheepRepository, CheepRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
