using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Domain.Entities
{
    public class NewItem:BaseEntity
    {
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? ContentHtml { get; set; } // detaylı içerik HTML olarak da alınabilir
        public DateTime? PublishedDateTime { get; set; }
    }
}
