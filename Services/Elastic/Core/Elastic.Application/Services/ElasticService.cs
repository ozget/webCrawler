
using Microsoft.Extensions.Logging;
using Elastic.Domain.Entities;
using Elastic.Domain.Repositories;

namespace Elastic.Application.Services
{
    public class ElasticService : Service<NewEntity>, IElasticService
    {
        private readonly IElasticRepository _repository;
        private readonly ILogger<ElasticService> _logger;

        public ElasticService(IElasticRepository repository, ILogger<ElasticService> logger):base(repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task SaveAsync(NewEntity entity)
        {
            if ((string.IsNullOrWhiteSpace(entity.Title)))
            {
                throw new ArgumentException("Haber başlıgı veya id bulunamadı.", nameof(entity.Title));
            }

            bool exists = await _repository.ExistsAsync(entity.Title, entity.Id);
            if (exists)
            {
                // Kayıt zaten var, işlem yapılmadı
                _logger.LogWarning("Entity with Title {Title} and Id {Id} already exists.", entity.Title, entity.Id);

            }
            else
            {
                await _repository.SaveAsync(entity);
                _logger.LogInformation(" Elastic DB kaydoldu {Title}.", entity.Title);
            }

           
           
        }

        public Task<NewEntity?> GetByIdAsync(string id) => _repository.GetByIdAsync(id);
        public Task<NewEntity?> GetByTitleAsync(string title) => _repository.GetByTitleAsync(title);

        public async Task<IEnumerable<NewEntity>> GetAllAsync()
        {
            var searchResponse = await _repository.GetAllAsync();

            return searchResponse.ToList();
        }

        public async Task DeleteAllNewsAsync()
        {
            await _repository.DeleteAllAsync();
        }
    }
}
