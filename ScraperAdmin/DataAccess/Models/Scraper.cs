using ScraperAdmin.DataAccess.Enumerables;

namespace ScraperAdmin.DataAccess.Models
{
    public class Scraper
    {
        public Guid ScraperId { get; set; } 
        public ScraperStatus ScraperStatusId { get; set; } 
        public byte[] Imagen { get; set; } 
        public DateTime FechaUltimaEjecucion { get; set; } 
    }
}
