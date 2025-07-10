
using Microsoft.Extensions.Logging;
using Elastic.Domain.Entities;
using Elastic.Domain.Repositories;

namespace Elastic.Application.Services
{
    public class ElasticService : IElasticService
    {
        private readonly IElasticRepository _repository;
        private readonly ILogger<ElasticService> _logger;

        public ElasticService(IElasticRepository repository, ILogger<ElasticService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task SaveAsync(NewEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }

            await _repository.SaveAsync(entity);
            _logger.LogInformation("ElasticService: NewEntity saved successfully. Title: {Title}", entity.Title);
        }

        public Task<NewEntity?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);
        public Task<NewEntity?> GetByTitleAsync(string title) => _repository.GetByTitleAsync(title);

        public async Task<IEnumerable<NewEntity>> GetAllAsync()
        {
            var searchResponse = await _repository.GetAllAsync();

            return searchResponse.ToList();
        }
    }
}
