using OpenAI.Assistants;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Models.Documents;
using ScraperAdmin.DataAccess.Models.Entities;
using ScraperAdmin.DataAccess.Services;
using System.Text.Json;

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
                // Create a new thread for this parsing task
                var threadEntity = await _assistantService.CreateThreadAsync();

                // Add the HTML content as a message to the thread
                await _assistantService.AddMessageToThreadAsync(threadEntity.Id, htmlContent, MessageRole.User);

                // Create and run the assistant
                var runEntity = await _assistantService.CreateAndRunAssistantAsync(threadEntity.Id);

                // Wait for the run to complete
                while (runEntity.Status != OpenAI.Assistants.RunStatus.Completed)
                {
                    await Task.Delay(1000); // Wait for 1 second before checking again
                    runEntity = await _assistantService.GetRunAsync(threadEntity.Id, runEntity.Id);

                    // Handle required actions if any
                    if (runEntity.RequiredActions != null && runEntity.RequiredActions.Any())
                    {
                        foreach (var action in runEntity.RequiredActions)
                        {
                            // Here you would handle any required actions
                            // For now, we'll just log them
                            _logger.LogInformation($"Required action: {action.FunctionName} with arguments {action.FunctionArguments}");
                        }
                    }
                }
                #pragma warning restore OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                // Get the processed result
                var messageEntity = await _assistantService.GetLatestMessageAsync(threadEntity.Id);

                if (messageEntity != null && !string.IsNullOrEmpty(messageEntity.Content))
                {
                    // Parse the JSON content into Event objects
                    var eventContainer = JsonSerializer.Deserialize<EventContainer>(messageEntity.Content);
                    return eventContainer?.Eventos ?? new List<Event>();
                }
                else
                {
                    _logger.LogWarning("No content returned from AI assistant");
                    return new List<Event>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while parsing HTML to events");
                throw;
            }
        }

        private class EventContainer
        {
            public List<Event> Eventos { get; set; }
        }
    }
}