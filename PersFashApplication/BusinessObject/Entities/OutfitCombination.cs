using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class OutfitCombination
{
    public int OutfitId { get; set; }

    public int? CustomerId { get; set; }

    public int? TopItemId { get; set; }

    public int? BottomItemId { get; set; }

    public int? ShoesItemId { get; set; }

    public int? AccessoriesItemId { get; set; }

    public int? DressItemId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual FashionItem? AccessoriesItem { get; set; }

    public virtual FashionItem? BottomItem { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual FashionItem? DressItem { get; set; }

    public virtual FashionItem? ShoesItem { get; set; }

    public virtual FashionItem? TopItem { get; set; }
}
