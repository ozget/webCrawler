

using System.Linq.Expressions;

namespace Elastic.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task SaveAsync(T entity);
        Task<T?> GetByIdAsync(string id);
        
         Task<IEnumerable<T>> GetAllAsync();

    }
}
