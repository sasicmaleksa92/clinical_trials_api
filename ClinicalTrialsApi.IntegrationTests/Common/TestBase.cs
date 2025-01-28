using Microsoft.AspNetCore.Mvc.Testing;
using System.Reflection;

namespace ClinicalTrialsApi.IntegrationTests.Common
{
    [Collection("Integration Tests")]
    public abstract class TestBase(TestServer server) : IAsyncLifetime
    {
        private readonly TestServer _server = server;
        private AsyncServiceScope _scope;
        protected IServiceProvider _services;
        protected HttpClient _client;

        public async Task InitializeAsync()
        {
            _client = _server.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost/")
            });
            _scope = _server.Services.CreateAsyncScope();
            _services = _scope.ServiceProvider;

            var dataSeedService = _services.GetRequiredService<IDataSeedService>();
            await dataSeedService.SeedDataAsync();

        }

        public string ReadEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public async Task DisposeAsync()
        {
            var dataSeedService = _services.GetRequiredService<IDataSeedService>();
            await dataSeedService.DeleteDataAsync();
            await _scope.DisposeAsync();
        }
    }
}
