using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Services;

namespace ScraperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperTestController : ControllerBase
    {
        private readonly IScraperService _scraperService;

        public ScraperTestController(IScraperService scraperService)
        {
            _scraperService = scraperService;
        }

        // TODO: Remove this controller after the MVC layer is fully implemented.

        // GET: api/scraperTest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scraper>>> GetAllScrapers()
        {
            var scrapers = await _scraperService.GetAllScrapersAsync();
            return Ok(scrapers);
        }

        // GET: api/scraperTest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Scraper>> GetScraperById(Guid id)
        {
            var scraper = await _scraperService.GetScraperByIdAsync(id);
            if (scraper == null)
            {
                return NotFound($"Scraper with ID {id} not found.");
            }
            return Ok(scraper);
        }

        // POST: api/scraperTest/create
        [HttpPost("create")]
        public async Task<ActionResult> CreateScraper([FromBody] Scraper scraper)
        {
            if (scraper == null)
            {
                return BadRequest("Scraper data is required.");
            }

            await _scraperService.CreateScraperAsync(scraper);
            return Ok(scraper);
        }

        // PUT: api/scraperTest/update/{id}
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateScraper(Guid id, [FromBody] Scraper scraper)
        {
            if (id != scraper.ScraperId)
            {
                return BadRequest("Scraper ID mismatch.");
            }

            await _scraperService.UpdateScraperAsync(scraper);
            return Ok(scraper);
        }

        // DELETE: api/scraperTest/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteScraper(Guid id)
        {
            await _scraperService.DeleteScraperAsync(id);
            return Ok($"Scraper with ID {id} has been deleted.");
        }
    }
}
