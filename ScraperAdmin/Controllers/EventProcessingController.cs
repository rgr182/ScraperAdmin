using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Services;


namespace ScraperAdmin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventProcessingController : ControllerBase
    {
        private readonly IAIEventProcessingService _aiEventProcessingService;
        private readonly IRawHtmlService _rawHtmlService;
        private readonly ILogger<EventProcessingController> _logger;

        public EventProcessingController(
            IAIEventProcessingService aiEventProcessingService,
            IRawHtmlService rawHtmlService,
            ILogger<EventProcessingController> logger)
        {
            _aiEventProcessingService = aiEventProcessingService;
            _rawHtmlService = rawHtmlService;
            _logger = logger;
        }

        [HttpPost("process-all")]
        public async Task<IActionResult> ProcessAllEvents()
        {
            try
            {
                var rawHtmlDocuments = await _rawHtmlService.GetAllRawHtmlAsync();
                await _aiEventProcessingService.ProcessAndStoreEventsAsync(rawHtmlDocuments);
                return Ok($"Processed and stored events from {rawHtmlDocuments.Count()} documents");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing events");
                return StatusCode(500, "An error occurred while processing events");
            }
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