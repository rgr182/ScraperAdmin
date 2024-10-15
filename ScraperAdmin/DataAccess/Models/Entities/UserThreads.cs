using System.ComponentModel.DataAnnotations;

namespace ScraperAdmin.DataAccess.Models.Entities
{
    public class UserThread
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ThreadId { get; set; }
        public DateTime LastUsed { get; set; }
        public bool IsActive { get; set; }
    }
}