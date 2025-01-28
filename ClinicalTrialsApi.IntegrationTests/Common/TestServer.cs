using ClinicalTrials.Infrastructure.Persistance.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace ClinicalTrialsApi.IntegrationTests.Common
{
    public class TestServer : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "443");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                var root = Directory.GetCurrentDirectory();
                var fileProvider = new PhysicalFileProvider(root);
                config.AddJsonFile(fileProvider, "appsettings.test.json", false, false);
            });

            builder.ConfigureTestServices(services =>
            {
                var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ClinicalTrialDbContext));
                if (context != null)
                {
                    services.Remove(context);
                    var options = services.Where(r => r.ServiceType == typeof(DbContextOptions)
                      || r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)).ToArray();
                    foreach (var option in options)
                    {
                        services.Remove(option);
                    }
                }
                services.AddDbContext<ClinicalTrialDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryClinicalTrialDb");
                });

                services.AddTransient<IDataSeedService, DataSeedService>();

            });


        }
    }
}
