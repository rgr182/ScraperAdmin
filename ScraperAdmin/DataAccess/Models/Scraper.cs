using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ScraperAdmin.DataAccess.Enumerables;


namespace ScraperAdmin.DataAccess.Models
{
    public class Scraper
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid ScraperId { get; set; }

        [BsonElement("ScraperStatusId")]
        public ScraperStatus ScraperStatusId { get; set; }

        [BsonElement("ImagePath")]
        public string ImagePath { get; set; }

        [BsonElement("LastExecutionDate")]
        public DateTime? LastExecutionDate { get; set; }

        [BsonElement("ScraperName")]
        public string ScraperName { get; set; }

        [BsonElement("ScraperUrl")]
        public string ScraperUrl { get; set; }
    }
}