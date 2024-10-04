using System;
using System.Collections.Generic;

namespace ScraperAdmin.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int UserId { get; set; }

    public string UserToken { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public virtual User User { get; set; } = null!;
}
