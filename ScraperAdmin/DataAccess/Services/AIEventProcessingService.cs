using ScraperAdmin.DataAccess.Models.Documents;
using ScraperAdmin.DataAccess.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ScraperAdmin.DataAccess.Services
{
   

    public class AIEventProcessingService : IAIEventProcessingService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IParserService _parserService;
        private readonly IRawHtmlRepository _rawHtmlRepository;
        private readonly IScraperRepository _scraperRepository;
        private readonly ILogger<AIEventProcessingService> _logger;

        public AIEventProcessingService(
            IEventRepository eventRepository, 
            IParserService parserService,
            IRawHtmlRepository rawHtmlRepository,
            IScraperRepository scraperRepository,
            ILogger<AIEventProcessingService> logger)
        {
            _eventRepository = eventRepository;
            _parserService = parserService;
            _rawHtmlRepository = rawHtmlRepository;
            _scraperRepository = scraperRepository;
            _logger = logger;
        }

        public async Task ProcessAndStoreEventsAsync()
        {
            try
            {
                var unparsedDocuments = await _rawHtmlRepository.GetAllUnparsedRawHtmlAsync();
                foreach (var document in unparsedDocuments)
                {
                    _logger.LogInformation("Processing document: {DocumentId}", document.Id);
                    
                    try
                    {
                        if (await _eventRepository.ScraperExistsAsync(document.ScraperId))
                        {
                        var parseResult = await _parserService.ParseHtmlToEventsAsync(document);
                            await _eventRepository.CreateEventsAsync(parseResult.Events);
                            await _rawHtmlRepository.UpdateParsedStatusAsync(document.Id, true);
                            await _scraperRepository.UpdateLastExecutionDateAsync(document.ScraperId, DateTime.UtcNow);
                            
                            _logger.LogInformation("Processed and stored {EventCount} events for document: {DocumentId}, Scraper: {ScraperId}", 
                                parseResult.Events.Count, document.Id, document.ScraperId);
                        }
                        else
                        {
                            _logger.LogWarning("Skipped processing for invalid scraper: {ScraperId} in document: {DocumentId}", 
                                document.ScraperId, document.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing document: {DocumentId}", document.Id);
                    }
                }
                _logger.LogInformation("Completed processing all unparsed documents");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing and storing events");
                throw;
            }
        }
    }
}