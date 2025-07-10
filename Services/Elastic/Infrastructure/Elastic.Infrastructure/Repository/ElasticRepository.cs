using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elastic.Domain.Entities;
using Elastic.Domain.Repositories;
using Nest;

namespace Elastic.Infrastructure.Repository
{
    public class ElasticRepository : IElasticRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _indexName = "news";

        public ElasticRepository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task SaveAsync(NewEntity entity)
        {
            var response = await _elasticClient.IndexAsync(entity, i => i.Index(_indexName));

            if (!response.IsValid)
            {
                throw new Exception($"Elasticsearch error: {response.OriginalException?.Message}");
            }
        }

        public async Task<NewEntity?> GetByIdAsync(string id)
        {
            var response = await _elasticClient.GetAsync<NewEntity>(id, g => g.Index(_indexName));
            return response.Found ? response.Source : null;
        }

        public async Task<IEnumerable<NewEntity>> GetAllAsync()
        {
            var searchResponse = await _elasticClient.SearchAsync<NewEntity>(s => 
                                                                s.Index(_indexName)
                                                                .MatchAll()
                                                                .Size(1000));
            return searchResponse.Documents;
        }

        public async Task<NewEntity?> GetByTitleAsync(string title)
        {
            var response = await _elasticClient.SearchAsync<NewEntity>(s => s
                .Index(_indexName)
                .Query(q => q.Match(m => m.Field(f => f.Title).Query(title)))
                .Size(1));

            return response.Documents.FirstOrDefault();
        }
      
    }
}
