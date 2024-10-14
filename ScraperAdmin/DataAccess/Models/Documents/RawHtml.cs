using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScraperAdmin.DataAccess.Models.Documents
{
    public class RawHtmlDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string title { get; set; }

        public string general { get; set; }
        [BsonElement("isParsed")]
        [BsonRepresentation(BsonType.Boolean)]
        public bool IsParsed { get; set; } = false;

        [BsonElement("scraperId")]
        public Guid ScraperId { get; set; }
    }
}