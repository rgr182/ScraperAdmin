namespace ScraperAdmin.DataAccess.Models.DTOs
{
    public class ChatStartResultDto
    {
        public string ThreadId { get; set; }
        public string WelcomeMessage { get; set; }
        public List<MessageDto> Messages { get; set; }
    }

    public class ChatRequestDto
    {
        public string ThreadId { get; set; }
        public string UserMessage { get; set; }
    }

    public class ChatResponseDto
    {
        public string ThreadId { get; set; }
        public string Response { get; set; }
    }
}