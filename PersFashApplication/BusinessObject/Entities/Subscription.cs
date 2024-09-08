using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Subscription
{
    public int SubscriptionId { get; set; }

    public string SubscriptionTitle { get; set; } = null!;

    public decimal? Price { get; set; }

    public int? DurationInDays { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<CustomerSubscription> CustomerSubscriptions { get; set; } = new List<CustomerSubscription>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
