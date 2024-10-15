using ScraperAdmin.DataAccess.Models.Documents;
namespace ScraperAdmin.DataAccess.Models.DTOs
{
public class ParseResult
    {
        public Guid ScraperId { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
    }
}