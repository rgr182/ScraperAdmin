using ScraperAdmin.DataAccess.Enumerables;

public class Scraper
{
    public Guid ScraperId { get; set; }
    public ScraperStatus ScraperStatusId { get; set; }    
    public string ImagePath { get; set; }
    public DateTime? LastExecutionDate { get; set; }
    public string ScrapperName { get; set; }        
    public string ScraperUrl {  get; set; }
}
