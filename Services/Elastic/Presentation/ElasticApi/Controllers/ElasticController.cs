using Elastic.Application.Services;
using Elastic.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ElasticApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElasticController : ControllerBase
    {
        private readonly IElasticService _service;

        public ElasticController(IElasticService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewEntity entity)
        {
           
            await _service.SaveAsync(entity);
            return Ok("Saved");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var result = await _service.GetByTitleAsync(title);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] string? query=null, [FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Geçersiz sayfa veya sayfa boyutu.");
            var result = await _service.GetAllAsync(query, page, pageSize);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
             await _service.DeleteAllNewsAsync();
            return Ok();
        }
    }
}
