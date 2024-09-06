using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class WardrobeItem
{
    public int WardrobeItemId { get; set; }

    public int? WardrobeId { get; set; }

    public int? ItemId { get; set; }

    public virtual FashionItem? Item { get; set; }

    public virtual Wardrobe? Wardrobe { get; set; }
}
