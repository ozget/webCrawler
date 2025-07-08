using Crawler.Domain.Entities;

namespace Elastic.Domain.Entities
{
    public class NewEntity : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Link { get; set; } = default!;

        public string? PublishDate { get; set; }
        public string? ImageUrl { get; set; }

        public virtual NewItem Details { get; set; }

        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
