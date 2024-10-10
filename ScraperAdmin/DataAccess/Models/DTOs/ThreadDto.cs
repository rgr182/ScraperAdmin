namespace ScraperAdmin.DataAccess.Models.DTOs
{
    public record UserThreadDto(
        int Id,
        string ThreadId,
        DateTime LastUsed,
        bool IsActive
    );
}