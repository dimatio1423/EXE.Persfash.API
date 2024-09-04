using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class CustomerProfile
{
    public int ProfileId { get; set; }

    public int? CustomerId { get; set; }

    public string? BodyType { get; set; }

    public string? FashionStyle { get; set; }

    public string? PreferredColors { get; set; }

    public string? PreferredMaterials { get; set; }

    public string? Occasion { get; set; }

    public string? Lifestyle { get; set; }

    public string? SocialMediaLinks { get; set; }

    public bool? ProfileSetupComplete { get; set; }

    public virtual Customer? Customer { get; set; }
}
