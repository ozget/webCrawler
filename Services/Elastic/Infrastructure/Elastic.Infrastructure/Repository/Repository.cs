using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Elastic.Domain.Repositories;
using Nest;

namespace Elastic.Infrastructure.Repository
{
    public class Repository<T> : Domain.Repositories.IRepository<T> where T : class
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _indexName;

        public Repository(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
            _indexName = typeof(T).Name.ToLower();
        }

        public async Task SaveAsync(T entity) =>
            await _elasticClient.IndexAsync(entity, i => i.Index(_indexName));

        public async Task<T?> GetByIdAsync(string id)
        {
            var response = await _elasticClient.GetAsync<T>(id, g => g.Index(_indexName));
            return response.Found ? response.Source : null;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var searchResponse = await _elasticClient.SearchAsync<T>(s => s.Index(_indexName).MatchAll().Size(1000));
            return searchResponse.Documents;
        }
    

       

      

       
    }
}
