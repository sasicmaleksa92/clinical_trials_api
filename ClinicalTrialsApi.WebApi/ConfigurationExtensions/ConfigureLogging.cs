using Destructurama;
using Serilog;

namespace ClinicalTrialsApi.WebApi.ConfigurationExtensions
{
    public static class ConfigureLoggingExtensions
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                   .Destructure.UsingAttributes()
                   .ReadFrom.Configuration(builder.Configuration)
                   .Enrich.FromLogContext()
                   .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddHttpLogging(options =>
            {
                options.CombineLogs = true;
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPath
                    | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestQuery
                    | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseStatusCode
                    | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseBody
                    | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Duration;
            });
        }
    }
}
