using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Wardrobe
{
    public int WardrobeId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? DateAdded { get; set; }

    public string? Notes { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<WardrobeItem> WardrobeItems { get; set; } = new List<WardrobeItem>();
}
