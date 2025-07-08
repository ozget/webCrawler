
using Elastic.Domain.Entities;

namespace Elastic.Domain.Repositories
{
    public interface IElasticRepository : IRepository<NewEntity>
    {
        Task<NewEntity?> GetByTitleAsync(string title); 
    }
}
