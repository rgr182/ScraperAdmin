using System.Text.Json.Serialization;

namespace ScraperAdmin.DataAccess.Models.DTOs
{
    public record EventJsonDto
    {
        [JsonPropertyName("titulo")]
        public required string Titulo { get; init; }

        [JsonPropertyName("descripcion")]
        public required string Descripcion { get; init; }

        [JsonPropertyName("lugar")]
        public required string Lugar { get; init; }

        [JsonPropertyName("fecha")]
        public required string Fecha { get; init; }

        [JsonPropertyName("horario")]
        public required string Horario { get; init; }

        [JsonPropertyName("ligaDetalle")]
        public required string LigaDetalle { get; init; }
    }
}