using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.EntityFrameworkCore;

Main.Program.BuildAndRun(args);

namespace Main
{
    public partial class Program
    {
        public static WebApplication BuildAndRun(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Chirp")));
            builder.Services.AddScoped<ICheepRepository, CheepRepository>();

            var app = builder.Build();

            // Seed database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ChirpDBContext>();
                context.Database.Migrate();

                //Then you can use the context to seed the database for example
                DbInitializer.SeedDatabase(context);
            }

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

            return app;
        }
    }
}