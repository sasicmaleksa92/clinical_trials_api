using ClinicalTrials.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrialsApi.IntegrationTests.Common
{
    public class DataSeedService : IDataSeedService
    {
        private readonly ClinicalTrialDbContext _context;

        public DataSeedService(ClinicalTrialDbContext context)
        {
            _context = context;
        }

        public async Task DeleteDataAsync()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        public async Task SeedDataAsync()
        {
            await SeedGameChoicesAsync();
            await _context.SaveChangesAsync();
        }

        public async Task SeedGameChoicesAsync()
        {
            if (!await _context.ClinicalTrials.AnyAsync())
            {
                _context.ClinicalTrials.AddRange(TestData.ClinicalTrialsTestList);
            }
        }

    }
}
