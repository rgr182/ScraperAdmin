using System.Text.Json;
using OpenAI.Assistants;
using ScraperAdmin.DataAccess.Models.Documents;
using ScraperAdmin.DataAccess.Models.Entities;
using MongoDB.Bson;
using ScraperAdmin.DataAccess.Models.DTOs;

namespace ScraperAdmin.DataAccess.Services
{
    public class ParserService : IParserService
    {
        private readonly IAssistantService _assistantService;
        private readonly ILogger<ParserService> _logger;

        public ParserService(IAssistantService assistantService, ILogger<ParserService> logger)
        {
            _assistantService = assistantService;
            _logger = logger;
        }

        public async Task<ParseResult> ParseHtmlToEventsAsync(RawHtmlDocument document)
        {
            try
            {
                var threadEntity = await _assistantService.CreateThreadAsync();
                #pragma warning disable OPENAI001
                await _assistantService.AddMessageToThreadAsync(threadEntity.Id, document.general, MessageRole.User);
                var runEntity = await _assistantService.CreateAndRunAssistantAsync(threadEntity.Id);

                List<Event> parsedEvents = new List<Event>();

                while (runEntity.Status != RunStatus.Completed && runEntity.Status != RunStatus.Failed)
                {
                    await Task.Delay(1000);
                    runEntity = await _assistantService.GetRunAsync(threadEntity.Id, runEntity.Id);
                    if (runEntity.Status == RunStatus.RequiresAction)
                    {
                        var newEvents = await HandleRequiredActions(threadEntity.Id, runEntity, scraperId: document.ScraperId);
                        parsedEvents.AddRange(newEvents);
                    }
                }

                if (runEntity.Status == RunStatus.Failed)
                {
                    throw new Exception("Assistant run failed");
                }
                #pragma warning restore OPENAI001 
                return new ParseResult
                {
                    ScraperId = document.ScraperId,
                    Events = parsedEvents
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while parsing HTML to events for document {DocumentId}", document.Id);
                throw;
            }
        }

        private async Task<List<Event>> HandleRequiredActions(string threadId, DataAccess.Models.Entities.Run runEntity, Guid? scraperId = null)
        {
            List<Event> parsedEvents = new List<Event>();

            foreach (var action in runEntity.RequiredActions)
            {
                if (action.FunctionName == "store_parsed_events")
                {
                    _logger.LogInformation("Storing parsed events: {ThreadID}, {RunID}", threadId, runEntity.Id);
                    var parsedJson = action.FunctionArguments;
                    _logger.LogInformation("Parsed JSON: {ParsedJSON}", parsedJson);
                    var events = ParseJsonContent(parsedJson, scraperId);
                    parsedEvents.AddRange(events);
                    _logger.LogInformation("Parsed events: {ParsedEvents}", JsonSerializer.Serialize(events));
                    
                    var response = JsonSerializer.Serialize(new { success = true, count = events.Count });
                    _logger.LogInformation("Submitting response to complete required action: {ThreadID}, {RunID}, {ToolCallID}, {Response}", 
                        threadId, runEntity.Id, action.ToolCallId, response);
                    await _assistantService.SubmitToolOutputsToRunAsync(threadId, runEntity.Id, action.ToolCallId, response);
                }
            }

            return parsedEvents;
        }

        private List<Event> ParseJsonContent(string content, Guid? scraperId = null)
        {
            try
            {
                _logger.LogInformation("Parsing JSON content: {Content}", content);

                using JsonDocument doc = JsonDocument.Parse(content);
                string unescapedJson = doc.RootElement.GetProperty("eventos").ToString();

                var eventos = JsonSerializer.Deserialize<List<EventJsonDto>>(unescapedJson);
                var events = eventos?.Select(e => new Event
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Title = e.Titulo,
                    Description = e.Descripcion,
                    Location = e.Lugar,
                    Date = string.IsNullOrWhiteSpace(e.Fecha) ? DateTime.UtcNow : DateTime.Parse(e.Fecha),
                    Time = e.Horario,
                    DetailLink = e.LigaDetalle,
                    ScraperId = scraperId
                }).ToList() ?? new List<Event>();

                _logger.LogInformation("Parsed {Count} events from JSON", events.Count);
                return events;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON content: {Content}", content);
                return new List<Event>();
            }
        }
    }
}