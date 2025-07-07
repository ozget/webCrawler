using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Domain.Entities;

namespace Crawler.Application.IServices
{
    public interface ICrawlerService
    {
        Task<NewEntity> FetchNewAsync();

    }
}
