using ScraperAdmin.DataAccess.Enumerables;

namespace ScraperAdmin.DataAccess.Models
{
    public class Scraper
    {
        public Guid ScraperId { get; set; }
        public ScraperStatus ScraperStatusId { get; set; }
        public string ImagePath { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public string ScraperName { get; set; }
        public string ScraperUrl { get; set; }
    }
}