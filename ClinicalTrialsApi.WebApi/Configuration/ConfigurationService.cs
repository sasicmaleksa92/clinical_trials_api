namespace ClinicalTrialsApi.WebApi.Configuration
{
    public class ConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
            MaxFileSizeInMb = _configuration.GetSection("AppSettings").GetValue<int>("MaxFileSizeInMb")!;
        }

        public int MaxFileSizeInMb { get; set; }
    }

}
