using OpenAI.Assistants;
using ScraperAdmin.DataAccess.Models.Entities;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IAssistantService
    {
        Task<Models.Entities.Thread> CreateThreadAsync();
        #pragma warning disable OPENAI001 
        Task AddMessageToThreadAsync(string threadId, string content, MessageRole role);
        #pragma warning restore OPENAI001 
        Task<Run> CreateAndRunAssistantAsync(string threadId);
        Task<Run> GetRunAsync(string threadId, string runId);
        Task SubmitToolOutputsToRunAsync(string threadId, string runId, string toolCallId, string output);
        Task<OpenAIMessage?> GetLatestMessageAsync(string threadId);
    }
}