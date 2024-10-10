using System.Text.Json.Serialization;

namespace ScraperAdmin.DataAccess.Models.DTOs
{
    public record MessageDto(
        string Content,
        string Role,
        [property: JsonPropertyName("timestamp")] DateTime Timestamp
    );
}