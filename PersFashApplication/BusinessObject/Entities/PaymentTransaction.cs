using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class PaymentTransaction
{
    public int TransactionId { get; set; }

    public int PaymentId { get; set; }

    public int InfluencerId { get; set; }

    public decimal OriginalAmount { get; set; }

    public decimal? CommissionAmount { get; set; }

    public decimal TransferredAmount { get; set; }

    public DateTime? TransferDate { get; set; }

    public string? Status { get; set; }

    public virtual FashionInfluencer Influencer { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;
}
