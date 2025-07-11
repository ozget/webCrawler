using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Application.IServices;
using Microsoft.Extensions.Logging;

namespace Crawler.Infrastructure.Worker
{
    public class CrawlerScopedProcessor : ICrawlerScopedProcessor
    {
        private readonly ICrawlerService _crawlerService;
        private readonly ILogger<CrawlerScopedProcessor> _logger;

        public CrawlerScopedProcessor(ICrawlerService crawlerService, ILogger<CrawlerScopedProcessor> logger)
        {
            _crawlerService = crawlerService;
            _logger = logger;
        }

        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scoped işlem başlatıldı.");
            await _crawlerService.FetchNewAsync();
        }
    }
}
