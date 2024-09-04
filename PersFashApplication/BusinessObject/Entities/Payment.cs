﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public DateTime PayementDate { get; set; }

    public decimal Price { get; set; }

    public int CustomerId { get; set; }

    public int SubscriptionId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Subscription Subscription { get; set; } = null!;
}
