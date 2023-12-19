using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chirp.Tests;

// Class taken from https://danieldonbavand.com/2022/06/13/using-playwright-with-the-webapplicationfactory-to-test-a-blazor-application/
public class E2EWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private IHost? _host;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Same configuration as for integration tests, but done after first build so config isn't run twice
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ChirpDBContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<ChirpDBContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "E2ETestsDB1");
            });

            services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });
        });

        // Create the host for TestServer now before we  
        // modify the builder to use Kestrel instead.    
        var testHost = builder.Build();

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ChirpDBContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<ChirpDBContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "E2ETestsDB2");
            });
        });

        // Modify the host builder to use Kestrel instead  
        // of TestServer so we can listen on a real address.    

        builder.UseEnvironment("Development");

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        // Create and start the Kestrel server before the test server,  
        // otherwise due to the way the deferred host builder works    
        // for minimal hosting, the server will not get "initialized    
        // enough" for the address it is listening on to be available.    
        // See https://github.com/dotnet/aspnetcore/issues/33846.    

        _host = builder.Build();
        _host.Start();

        // Extract the selected dynamic port out of the Kestrel server  
        // and assign it onto the client options for convenience so it    
        // "just works" as otherwise it'll be the default http://localhost    
        // URL, which won't route to the Kestrel-hosted HTTP server.     

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(x => new Uri(x))
            .Last();

        // Return the host that uses TestServer, rather than the real one.  
        // Otherwise the internals will complain about the host's server    
        // not being an instance of the concrete type TestServer.    
        // See https://github.com/dotnet/aspnetcore/pull/34702.   

        testHost.Start();
        return testHost;
    }

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    private void EnsureServer()
    {
        if (_host is null)
        {
            // This forces WebApplicationFactory to bootstrap the server  
            using var _ = CreateDefaultClient();
        }
    }
}