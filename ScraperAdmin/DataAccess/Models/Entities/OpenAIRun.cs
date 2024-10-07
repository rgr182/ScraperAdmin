using OpenAI.Assistants;
namespace ScraperAdmin.DataAccess.Models.Entities
{
     public class RequiredActionEntity
    {
        public string ToolCallId { get; set; }
        public string FunctionName { get; set; }
        public string FunctionArguments { get; set; }
    }
    public class RunEntity
    {
        public string Id { get; set; }
        #pragma warning disable OPENAI001 // Using experimental OpenAI Assistant API
        public RunStatus Status { get; set; }
        #pragma warning restore OPENAI001
        public List<RequiredActionEntity> RequiredActions { get; set; }
    }
}