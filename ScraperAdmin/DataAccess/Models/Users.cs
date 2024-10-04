using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScraperAdmin.DataAccess.Models
{
    [Table("Users")]  // Mapea el modelo a la tabla "Users"
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }  // Mapea a la columna "UserId" en la base de datos

        [Required]
        [Column("Email")]
        public string Email { get; set; }  // Mapea a la columna "Email"

        [Required]
        [Column("Username")]
        public string Username { get; set; }  // Mapea a la columna "Username"

        [Required]
        [Column("AccessToken")]
        public string AccessToken { get; set; }  // Mapea a la columna "AccessToken"
        
        [Column("CreatedAt")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]  // El valor de "CreatedAt" será gestionado por la base de datos
        public DateTime CreatedAt { get; set; }  // Mapea a la columna "CreatedAt"     
    }
}
