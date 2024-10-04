using System;
using System.Collections.Generic;

namespace ScraperAdmin.Models;

public partial class PasswordRecoveryRequest
{
    public int PasswordRecoveryRequestId { get; set; }

    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime ExpirationTime { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
