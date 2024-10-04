using System;
using System.Collections.Generic;

namespace ScraperAdmin.Models;

public partial class Message
{
    public int Id { get; set; }

    public int UserThreadId { get; set; }

    public string Content { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual UserThread UserThread { get; set; } = null!;
}
