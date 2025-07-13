using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elastic.Domain.Entities;

namespace Elastic.Application.Services
{
    public interface IElasticService:IService<NewEntity>
    {
        Task<NewEntity?> GetByIdAsync(string id);
        Task<NewEntity?> GetByTitleAsync(string title);
        Task<IEnumerable<NewEntity>> GetAllAsync(string query, int page, int pageSize);

        Task DeleteAllNewsAsync();
    }
}
