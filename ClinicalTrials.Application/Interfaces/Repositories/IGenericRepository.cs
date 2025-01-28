using System.Linq.Expressions;

namespace ClinicalTrials.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> filter);

        //Aleksa
        Task<List<T>> GetAsync(
           string includeProperties = "",
           Expression<Func<T, bool>> filter = null);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task<IList<T>> AddManyAsync(IList<T> entities, CancellationToken cancellationToken);
    }
}
