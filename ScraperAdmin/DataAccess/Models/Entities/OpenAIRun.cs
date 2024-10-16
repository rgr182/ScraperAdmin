using OpenAI.Assistants;
namespace ScraperAdmin.DataAccess.Models.Entities
{
     public class RequiredAction
    {
        public string ToolCallId { get; set; }
        public string FunctionName { get; set; }
        public string FunctionArguments { get; set; }
    }
    public class Run
    {
        public string Id { get; set; }
        #pragma warning disable OPENAI001 // Using experimental OpenAI Assistant API
        public RunStatus Status { get; set; }
        #pragma warning restore OPENAI001
        public List<RequiredAction> RequiredActions { get; set; }
    }
}