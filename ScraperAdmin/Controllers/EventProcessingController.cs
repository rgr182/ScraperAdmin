using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Services;
using ScraperAdmin.Services;

namespace ScraperAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventProcessingController : ControllerBase
    {
        private readonly IAIEventProcessingService _aiEventProcessingService;
        private readonly ILogger<EventProcessingController> _logger;

        public EventProcessingController(
            IAIEventProcessingService aiEventProcessingService,
            ILogger<EventProcessingController> logger)
        {
            _aiEventProcessingService = aiEventProcessingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessEvents([FromBody] string htmlContent)
        {
            try
            {
                await _aiEventProcessingService.ProcessAndStoreEventsAsync(htmlContent);
                return Ok("Events processed and stored successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing events");
                return StatusCode(500, "An error occurred while processing events");
            }
        }
    }
}