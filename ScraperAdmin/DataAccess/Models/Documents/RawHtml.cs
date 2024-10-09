using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ScraperAdmin.DataAccess.Models.Documents{

    public class RawHtmlDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    public string? title { get; set; }
    public string general { get; set; }
}
}