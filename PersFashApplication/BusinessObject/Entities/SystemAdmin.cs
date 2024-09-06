using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class SystemAdmin
{
    public int AdminId { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
