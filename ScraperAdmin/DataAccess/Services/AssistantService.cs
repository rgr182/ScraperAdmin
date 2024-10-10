using OpenAI.Assistants;
using OpenAI;
using ScraperAdmin.DataAccess.Models.Entities;

namespace ScraperAdmin.DataAccess.Services
{
    public class AssistantService : IAssistantService
    {
        private readonly OpenAIClient _openAiClient;
        private readonly string? _assistantId;
        private readonly ILogger<AssistantService> _logger;

        public AssistantService(IConfiguration configuration, ILogger<AssistantService> logger)
        {
            string? apiKey = Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OpenAI API key is not set in environment variables.");
            }
            _openAiClient = new OpenAIClient(apiKey);
            _assistantId = configuration["OpenAI:AssistantId"];
            if (string.IsNullOrEmpty(_assistantId))
            {
                throw new InvalidOperationException("OpenAI Assistant ID is not set in configuration.");
            }
            _logger = logger;
        }
        
        public async Task<Models.Entities.Thread> CreateThreadAsync()
        {
            #pragma warning disable OPENAI001
            var thread = await _openAiClient.GetAssistantClient().CreateThreadAsync();
            #pragma warning restore OPENAI001
            return new Models.Entities.Thread { Id = thread.Value.Id };
        }

        #pragma warning disable OPENAI001 
        public async Task AddMessageToThreadAsync(string threadId, string content, MessageRole role)
      
        {
            
            await _openAiClient.GetAssistantClient().CreateMessageAsync(threadId, role, [content]);
        }
            #pragma warning restore OPENAI001

        public async Task<Run> CreateAndRunAssistantAsync(string threadId)
        {
            #pragma warning disable OPENAI001
            var run = await _openAiClient.GetAssistantClient().CreateRunAsync(threadId, _assistantId);
            #pragma warning restore OPENAI001
            return new Run { Id = run.Value.Id, Status = run.Value.Status };
        }

        public async Task<Run> GetRunAsync(string threadId, string runId)
        {
            #pragma warning disable OPENAI001
            var run = await _openAiClient.GetAssistantClient().GetRunAsync(threadId, runId);
            #pragma warning restore OPENAI001
            return new Run
            { 
                Id = run.Value.Id, 
                Status = run.Value.Status,
                RequiredActions = run.Value.RequiredActions?.Select(a => new Models.Entities.RequiredAction
                {
                    ToolCallId = a.ToolCallId,
                    FunctionName = a.FunctionName,
                    FunctionArguments = a.FunctionArguments
                }).ToList() ?? new List<Models.Entities.RequiredAction>()
            };
        }

        public async Task SubmitToolOutputsToRunAsync(string threadId, string runId, string toolCallId, string output)
        {
            #pragma warning disable OPENAI001
            _logger.LogInformation("Submitting tool outputs to run: {ThreadID}, {RunID}, {ToolCallID}, {Output}", threadId, runId, toolCallId, output);
            await _openAiClient.GetAssistantClient().SubmitToolOutputsToRunAsync(
                threadId,
                runId,
                [new ToolOutput(toolCallId, output)]
            );
            #pragma warning restore OPENAI001
        }
        //TODO, check if they have added an awaiter for GetMessagesAsync (they haven't as of october 7th 2024)
        public async Task<OpenAIMessage?> GetLatestMessageAsync(string threadId)
        {
            #pragma warning disable OPENAI001
            // var messages = await _openAiClient.GetAssistantClient().GetMessagesAsync(threadId);
            var messages =  _openAiClient.GetAssistantClient().GetMessagesAsync(threadId);
            var latestMessage = messages.ToBlockingEnumerable().FirstOrDefault();
            #pragma warning restore OPENAI001
        
            return latestMessage != null
                ? new OpenAIMessage { Content = latestMessage.Content?.FirstOrDefault()?.Text ?? string.Empty }
                : null;
        }
    }
}