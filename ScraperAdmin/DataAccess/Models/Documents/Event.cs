using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ScraperAdmin.DataAccess.Models.Documents
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("titulo")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("descripcion")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("lugar")]
        public string Location { get; set; }= string.Empty;

        [BsonElement("fecha")]
        public DateTime Date { get; set; }

        [BsonElement("horario")]
        public string Time { get; set; } = string.Empty;

        [BsonElement("liga_detalle")]
        public string DetailLink { get; set; } = string.Empty;
    }
}