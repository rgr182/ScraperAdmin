using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ScraperAdmin.DataAccess.Models
{
    [Table("Users")]  // Maps the model to the "Users" table
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int UserId { get; set; }  // Maps to the "UserId" column in the database

        [Column("Email")]
        public string Email { get; set; }  // Maps to the "Email" column

        [Required]
        [Column("Username")]
        public string Username { get; set; }  // Maps to the "Username" column

        [JsonIgnore]  // Prevents AccessToken from being exposed in JSON
        [Column("AccessToken")]
        public string? AccessToken { get; set; }  // Makes AccessToken optional in the model

        [Column("CreatedAt")]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]  // "CreatedAt" value will be managed by the database
        public DateTime CreatedAt { get; set; }  // Maps to the "CreatedAt" column
    }
}
