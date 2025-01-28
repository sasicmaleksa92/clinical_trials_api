namespace ClinicalTrialsApi.IntegrationTests.Common
{
    interface IDataSeedService
    {
        Task SeedDataAsync();

        Task DeleteDataAsync();
    }
}
