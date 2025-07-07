using Crawler.Application.IServices;
using Crawler.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CrawlerApi.Controllers
{
   
        [ApiController]
        [Route("api/[controller]")]
        public class CrawlerController : ControllerBase
        {
            private readonly ICrawlerService _crawlerService;

            public CrawlerController(ICrawlerService crawlerService)
            {
                _crawlerService = crawlerService;
            }


            [HttpPost]
            public async Task<IActionResult> GetNewCrawl()
            {
                var result = await _crawlerService.FetchNewAsync();
                return Ok(result);
            }
        }
    
}
