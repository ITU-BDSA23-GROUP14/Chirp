using System.Data.Common;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chirp.Tests;

// The following class is adapted from https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ChirpDBContext>));

            if(dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<ChirpDBContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "DBMemoryIntegration");
            });
        });

        builder.UseEnvironment("Development");
    }
}