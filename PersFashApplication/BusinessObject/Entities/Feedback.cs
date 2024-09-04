using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? CustomerId { get; set; }

    public int? ItemId { get; set; }

    public int? CourseId { get; set; }

    public int? InfluencerId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? DateGiven { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual FashionInfluencer? Influencer { get; set; }

    public virtual FashionItem? Item { get; set; }
}
