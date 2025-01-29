using Microsoft.Extensions.Configuration;

namespace ClinicalTrials.Domain.Configuration
{
    public static class Configuration
    {
        private static IConfiguration _configuration;

        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static class AppSettings
        {
            public static int MaxFileSizeInMb => int.Parse(_configuration.GetSection("AppSettings:MaxFileSizeInMb")?.Value ?? "1");

            public static int UploadClinicalTrialFileMaxFileSizeInMb => int.Parse(_configuration.GetSection("AppSettings:UploadClinicalTrialFile_MaxFileSizeInMb")?.Value ?? "1");

            public static string UploadClinicalTrialFileAllowedExtensions => _configuration.GetSection("AppSettings:UploadClinicalTrialFile_AllowedExtensions")?.Value ?? ".json";

            public static string? UploadClinicalTrialFileJsonSchemaFileName => _configuration.GetSection("AppSettings:UploadClinicalTrialFile_JsonSchemaFileName").Value;
        }

    }

}
