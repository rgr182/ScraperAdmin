using ScraperAdmin.DataAccess.Repositories;
using ScraperAdmin.Services;

namespace ScraperAdmin.DataAccess.Services
{
    public class AIEventProcessingService : IAIEventProcessingService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IParserService _parserService;
        private readonly ILogger<AIEventProcessingService> _logger;

        public AIEventProcessingService(
            IEventRepository eventRepository, 
            IParserService parserService,
            ILogger<AIEventProcessingService> logger)
        {
            _eventRepository = eventRepository;
            _parserService = parserService;
            _logger = logger;
        }

        public async Task ProcessAndStoreEventsAsync(string htmlContent)
        {
            try
            {
                var events = await _parserService.ParseHtmlToEventsAsync(htmlContent);
                _logger.LogInformation($"Successfully parsed {events.Count} events");
                await _eventRepository.CreateEventsAsync(events);

                _logger.LogInformation($"Successfully processed and stored {events.Count} events");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing and storing events");
                throw;
            }
        }
    }
}