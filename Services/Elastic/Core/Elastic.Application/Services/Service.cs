
using Elastic.Domain.Repositories;

namespace Elastic.Application.Services
{
    public class Service<T> : IService<T> where T : class
    {
     
        private readonly IRepository<T> _repository;
        public Service(IRepository<T> repository)
        {
           
            _repository = repository;
        }
      

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

    
        public async Task SaveAsync(T entity)
        {
            await _repository.SaveAsync(entity);
        }

    }
}