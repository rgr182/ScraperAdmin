using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ScraperAdmin.DataAccess.Models
{
    [Table("Users")]  // Mapea el modelo a la tabla "Users"
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int UserId { get; set; }  // Mapea a la columna "UserId" en la base de datos

        [Column("Email")]
        public string Email { get; set; }  // Mapea a la columna "Email"

        [Required]
        [Column("Username")]
        public string Username { get; set; }  // Mapea a la columna "Username"

        [JsonIgnore]  // Evita que AccessToken sea expuesto en JSON
        [Column("AccessToken")]
        public string? AccessToken { get; set; }  // Hacemos que AccessToken sea opcional en el modelo

        [Column("CreatedAt")]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]  // El valor de "CreatedAt" será gestionado por la base de datos
        public DateTime CreatedAt { get; set; }  // Mapea a la columna "CreatedAt"
    }
}
