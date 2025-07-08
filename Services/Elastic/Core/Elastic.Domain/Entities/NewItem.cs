
using Elastic.Domain.Entities;

namespace Crawler.Domain.Entities
{
    public class NewItem:BaseEntity
    {
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? ContentHtml { get; set; }
        public DateTime? PublishedDateTime { get; set; }
    }
}
