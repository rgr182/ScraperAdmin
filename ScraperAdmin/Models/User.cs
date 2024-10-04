using System;
using System.Collections.Generic;

namespace ScraperAdmin.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public virtual ICollection<PasswordRecoveryRequest> PasswordRecoveryRequests { get; set; } = new List<PasswordRecoveryRequest>();

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual ICollection<UserThread> UserThreads { get; set; } = new List<UserThread>();
}
