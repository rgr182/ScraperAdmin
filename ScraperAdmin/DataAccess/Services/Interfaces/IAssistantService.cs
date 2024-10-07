using OpenAI.Assistants;
using ScraperAdmin.DataAccess.Models.Entities;

namespace ScraperAdmin.DataAccess.Services
{
    public interface IAssistantService
    {
        Task<ThreadEntity> CreateThreadAsync();
        #pragma warning disable OPENAI001 
        Task AddMessageToThreadAsync(string threadId, string content, MessageRole role);
        #pragma warning restore OPENAI001 
        Task<RunEntity> CreateAndRunAssistantAsync(string threadId);
        Task<RunEntity> GetRunAsync(string threadId, string runId);
        Task SubmitToolOutputsToRunAsync(string threadId, string runId, string toolCallId, string output);
        Task<MessageEntity?> GetLatestMessageAsync(string threadId);
    }
}