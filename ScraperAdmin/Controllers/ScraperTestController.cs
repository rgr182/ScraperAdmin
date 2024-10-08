using Microsoft.AspNetCore.Mvc;
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
    }
}
