

using Elastic.Domain.Entities;
using Elastic.Domain.Repositories;

namespace Elastic.Application.Services
{
    public class ElasticService : IElasticService
    {
        private readonly IElasticRepository _repository;

        public ElasticService(IElasticRepository repository)
        {
            _repository = repository;
        }

        public Task SaveAsync(NewEntity entity) => _repository.SaveAsync(entity);
        public Task<NewEntity?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);
        public Task<NewEntity?> GetByTitleAsync(string title) => _repository.GetByTitleAsync(title);
    }
}
