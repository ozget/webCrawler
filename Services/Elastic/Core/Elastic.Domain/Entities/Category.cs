

using Elastic.Domain.Entities;

namespace Crawler.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string? Name { get; set; }

        public ICollection<NewEntity>? NewEntities { get; set; }
    }
}
