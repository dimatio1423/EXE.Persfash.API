using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Recommendation
{
    public int RecommendationId { get; set; }

    public int? CustomerId { get; set; }

    public int? ItemId { get; set; }

    public string? RecommendationType { get; set; }

    public DateTime? RecommendationDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual FashionItem? Item { get; set; }
}
