using ClinicalTrials.Application.Common.Behaviours;
using ClinicalTrials.Application.Common.FileProcessing;
using ClinicalTrials.Application.Common.Validators;
using ClinicalTrials.Application.Interfaces;
using ClinicalTrials.Application.UseCases.ClinicalTrials.Commands.CreateClinicalTrialCommand;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace ClinicalTrials.Application.ServiceExtensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddValidatorsFromAssemblyContaining<CreateClinicalTrialCommandValidator>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped(typeof(JsonFileProcessor<>));
            services.AddScoped(typeof(JsonSchemaValidator));
            services.AddScoped<IFileReader, EmbeddedResourceReader>();

            return services;
        }
    }
}
