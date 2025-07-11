using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Application.Events
{
    public class NewFetchedEventDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string? ImageUrl { get; set; }
        public string? PublishDate { get; set; }
        public string CategoryName { get; set; }
    }
}
