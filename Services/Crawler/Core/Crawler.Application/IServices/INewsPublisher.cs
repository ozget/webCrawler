
using Crawler.Application.Events;
using Crawler.Domain.Entities;

namespace Crawler.Application.IServices
{
    public interface INewsPublisher<T>
    {
        Task PublishMessageAsync(T message, string queueName);
    }
}
