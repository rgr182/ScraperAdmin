namespace ScraperAdmin.DataAccess.Models.DTOs
{
    public record ChatStartResultDto(
        string ThreadId,
        string WelcomeMessage,
        List<MessageDto> Messages
    );

    public record ChatRequestDto(
        string ThreadId,
        string UserMessage
    );

    public record ChatResponseDto(
        string ThreadId,
        string Response
    );
}