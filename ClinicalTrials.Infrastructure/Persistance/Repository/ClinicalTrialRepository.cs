using ClinicalTrials.Application.Interfaces.Repositories;
using ClinicalTrials.Domain.Entities;
using ClinicalTrials.Infrastructure.Persistance.Context;

namespace ClinicalTrials.Infrastructure.Persistance.Repository
{
    public class ClinicalTrialRepository(ClinicalTrialDbContext dbContext) : GenericRepository<ClinicalTrial>(dbContext), IClinicalTrialRepository
    {
    }


}
