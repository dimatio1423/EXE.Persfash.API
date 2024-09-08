using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class CustomerSubscription
{
    public int CustomerSubscriptionId { get; set; }

    public int CustomerId { get; set; }

    public int SubscriptionId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;
}
