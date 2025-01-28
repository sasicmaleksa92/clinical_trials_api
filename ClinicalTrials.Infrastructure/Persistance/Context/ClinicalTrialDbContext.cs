using ClinicalTrials.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Infrastructure.Persistance.Context
{
    public class ClinicalTrialDbContext : DbContext
    {
        public ClinicalTrialDbContext(DbContextOptions<ClinicalTrialDbContext> options) : base(options)
        {
        }

        public DbSet<ClinicalTrial> ClinicalTrials { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClinicalTrialDbContext).Assembly);
        }

    }
}
