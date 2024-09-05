using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Partner
{
    public int PartnerId { get; set; }

    public string? PartnerName { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhone { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<FashionItem> FashionItems { get; set; } = new List<FashionItem>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
