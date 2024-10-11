using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Services;

namespace ScraperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperMetadataController : ControllerBase
    {
        private readonly IScraperService _scraperService;

        public ScraperMetadataController(IScraperService scraperService)
        {
            _scraperService = scraperService;
        }

        // GET: api/scraperTest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scraper>>> GetAllScrapers()
        {
            var scrapers = await _scraperService.GetAllScrapersAsync(Request);
            return Ok(scrapers);
        }

        // GET: api/scraperTest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Scraper>> GetScraperById(Guid id)
        {
            var scraper = await _scraperService.GetScraperByIdAsync(id, Request);
            if (scraper == null)
            {
                return NotFound($"Scraper with ID {id} not found.");
            }
            return Ok(scraper);
        }

        // POST: api/scraperTest
        [HttpPost]
        public async Task<ActionResult<Scraper>> CreateScraper([FromBody] Scraper scraper)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdScraper = await _scraperService.CreateScraperAsync(scraper, Request);
            return CreatedAtAction(nameof(GetScraperById), new { id = createdScraper.ScraperId }, createdScraper);
        }

        // PATCH: api/scraperTest/{id}/lastExecution
        [HttpPatch("{id}/lastExecution")]
        public async Task<IActionResult> UpdateLastExecutionDate(Guid id, [FromBody] DateTime lastExecutionDate)
        {
            var updated = await _scraperService.UpdateLastExecutionDateAsync(id, lastExecutionDate);
            if (!updated)
            {
                return NotFound($"Scraper with ID {id} not found.");
            }
            return NoContent();
        }
    }
}