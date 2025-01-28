using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ClinicalTrials.Infrastructure.Persistance.Context;
using ClinicalTrials.Application.Interfaces.Repositories;
using ClinicalTrials.Infrastructure.Persistance.Repository;

namespace ClinicalTrials.Infrastructure.ServiceExtensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ClinicalTrialDbContext>(options =>
            {
                options.UseSqlServer(connectionString, o => o.MigrationsAssembly("ClinicalTrials.Infrastructure"));
            });

            services.AddScoped<IClinicalTrialRepository, ClinicalTrialRepository>();

            return services;
        }
    }
}
