using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Services;

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

        [HttpPost("process")]
        public async Task<IActionResult> ProcessEvents()
        {
            try
            {
                await _aiEventProcessingService.ProcessAndStoreEventsAsync();
                return Ok("Processed and stored events from all unparsed documents");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing events");
                return StatusCode(500, "An error occurred while processing events");
            }
        }
    }
}