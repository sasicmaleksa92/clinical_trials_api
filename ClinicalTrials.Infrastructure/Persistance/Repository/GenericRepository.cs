using ClinicalTrials.Application.Interfaces.Repositories;
using ClinicalTrials.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicalTrials.Infrastructure.Persistance.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ClinicalTrialDbContext _dbContext;

        public GenericRepository(ClinicalTrialDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GetFirstAsync(Expression<Func<T, bool>> filter)
        {
            T? t = await _dbContext.Set<T>().FirstOrDefaultAsync(filter);
            return t;
        }

        public virtual async Task<List<T>> GetAsync(
           string includeProperties = "",
           Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var includeProperty in includeProperties
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))

            {
                query = query.Include(includeProperty);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();

        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IList<T>> AddManyAsync(IList<T> entities, CancellationToken cancellationToken)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

    }
}
