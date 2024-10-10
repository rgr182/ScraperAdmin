using System.Text.Json;
using OpenAI.Assistants;
using ScraperAdmin.DataAccess.Models.Documents;
using ScraperAdmin.DataAccess.Models.Entities;
using ScraperAdmin.DataAccess.Services;
using MongoDB.Bson;
using ScraperAdmin.DataAccess.Models.DTOs;

namespace ScraperAdmin.Services
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

        #pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public async Task<List<Event>> ParseHtmlToEventsAsync(string htmlContent)
        {
            try
            {
                var threadEntity = await _assistantService.CreateThreadAsync();
                _logger.LogInformation("Created thread: {ThreadID}", threadEntity.Id);
                await _assistantService.AddMessageToThreadAsync(threadEntity.Id, htmlContent, MessageRole.User);
                _logger.LogInformation("Added message to thread: {ThreadID}", threadEntity.Id);
                var runEntity = await _assistantService.CreateAndRunAssistantAsync(threadEntity.Id);
                _logger.LogInformation("Created and run assistant: {ThreadID}, {RunID}", threadEntity.Id, runEntity.Id);
                List<Event> parsedEvents = new List<Event>();

                while (runEntity.Status != RunStatus.Completed && runEntity.Status != RunStatus.Failed)
                {
                    _logger.LogInformation("Waiting for assistant to complete: {ThreadID}, {RunID}", threadEntity.Id, runEntity.Id);
                    await Task.Delay(1000);
                    runEntity = await _assistantService.GetRunAsync(threadEntity.Id, runEntity.Id);
                    _logger.LogInformation("Status: {Status}", runEntity.Status);
                    if (runEntity.Status == RunStatus.RequiresAction)
                    {
                        _logger.LogInformation("Handling required action: {ThreadID}, {RunID}", threadEntity.Id, runEntity.Id);
                        var newEvents = await HandleRequiredActions(threadEntity.Id, runEntity);
                        parsedEvents.AddRange(newEvents);
                    }
                }

                if (runEntity.Status == RunStatus.Failed)
                {
                    throw new Exception("Assistant run failed");
                }

                _logger.LogInformation("Total parsed events: {Count}", parsedEvents.Count);
                return parsedEvents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while parsing HTML to events");
                throw;
            }
        }

        private async Task<List<Event>> HandleRequiredActions(string threadId, Run runEntity)
        {
            List<Event> parsedEvents = new List<Event>();

            foreach (var action in runEntity.RequiredActions)
            {
                if (action.FunctionName == "store_parsed_events")
                {
                    _logger.LogInformation("Storing parsed events: {ThreadID}, {RunID}", threadId, runEntity.Id);
                    var parsedJson = action.FunctionArguments;
                    _logger.LogInformation("Parsed JSON: {ParsedJSON}", parsedJson);
                    var events = ParseJsonContent(parsedJson);
                    parsedEvents.AddRange(events);
                    _logger.LogInformation("Parsed events: {ParsedEvents}", JsonSerializer.Serialize(events));
                    
                    // Submit a response to complete the required action
                    var response = JsonSerializer.Serialize(new { success = true, count = events.Count });
                    _logger.LogInformation("Submitting response to complete required action: {ThreadID}, {RunID}, {ToolCallID}, {Response}", 
                        threadId, runEntity.Id, action.ToolCallId, response);
                    await _assistantService.SubmitToolOutputsToRunAsync(threadId, runEntity.Id, action.ToolCallId, response);
                }
            }

            return parsedEvents;
        }
        #pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        private List<Event> ParseJsonContent(string content)
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
                    DetailLink = e.LigaDetalle
                }).ToList() ?? new List<Event>();

                _logger.LogInformation("Parsed {Count} events from JSON", events.Count);
                foreach (var evt in events)
                {
                    _logger.LogInformation("Parsed event: {EventTitle}", evt.Title);
                }

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