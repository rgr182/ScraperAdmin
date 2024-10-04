using System;
using System.Collections.Generic;

namespace ScraperAdmin.Models;

public partial class UserThread
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ThreadId { get; set; } = null!;

    public DateTime LastUsed { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User User { get; set; } = null!;
}
