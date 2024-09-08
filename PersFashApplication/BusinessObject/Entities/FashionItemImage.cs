using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class FashionItemImage
{
    public int ItemImageId { get; set; }

    public int? ItemId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual FashionItem? Item { get; set; }
}
