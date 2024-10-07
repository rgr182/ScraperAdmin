
using System.Text.Json.Serialization;

namespace ScraperAdmin.DataAccess.Models.DTOs
{
public class MessageDto
    {
        public string Content { get; set; }
        public string Role { get; set; }
         [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
 }
     