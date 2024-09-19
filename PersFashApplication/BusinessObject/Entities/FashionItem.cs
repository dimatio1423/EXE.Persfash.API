using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class FashionItem
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Category { get; set; }

    public string? Brand { get; set; }

    public decimal? Price { get; set; }

    public string? FitType { get; set; }

    public string? GenderTarget { get; set; }

    public string? FashionTrend { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public string? Material { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? Occasion { get; set; }

    public string? ProductUrl { get; set; }

    public DateTime? DateAdded { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<FashionItemImage> FashionItemImages { get; set; } = new List<FashionItemImage>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OutfitCombination> OutfitCombinationAccessoriesItems { get; set; } = new List<OutfitCombination>();

    public virtual ICollection<OutfitCombination> OutfitCombinationBottomItems { get; set; } = new List<OutfitCombination>();

    public virtual ICollection<OutfitCombination> OutfitCombinationDressItems { get; set; } = new List<OutfitCombination>();

    public virtual ICollection<OutfitCombination> OutfitCombinationShoesItems { get; set; } = new List<OutfitCombination>();

    public virtual ICollection<OutfitCombination> OutfitCombinationTopItems { get; set; } = new List<OutfitCombination>();

    public virtual ICollection<OutfitFavorite> OutfitFavoriteAccessoriesItems { get; set; } = new List<OutfitFavorite>();

    public virtual ICollection<OutfitFavorite> OutfitFavoriteBottomItems { get; set; } = new List<OutfitFavorite>();

    public virtual ICollection<OutfitFavorite> OutfitFavoriteDressItems { get; set; } = new List<OutfitFavorite>();

    public virtual ICollection<OutfitFavorite> OutfitFavoriteShoesItems { get; set; } = new List<OutfitFavorite>();

    public virtual ICollection<OutfitFavorite> OutfitFavoriteTopItems { get; set; } = new List<OutfitFavorite>();

    public virtual ICollection<WardrobeItem> WardrobeItems { get; set; } = new List<WardrobeItem>();
}
