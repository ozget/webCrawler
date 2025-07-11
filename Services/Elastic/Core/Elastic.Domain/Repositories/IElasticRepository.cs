
using Crawler.Domain.Entities;
using Elastic.Domain.Entities;

namespace Elastic.Domain.Repositories
{
    public interface IElasticRepository : IRepository<NewEntity>
    {
        Task<NewEntity?> GetByTitleAsync(string title);

        Task<bool> ExistsAsync(string title, string id);
        Task IndexAsync(NewEntity newEntity);

        Task DeleteAllAsync();
    }
}
