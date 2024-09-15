using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class InfluencerPaymentInformation
{
    public int PaymentInformationId { get; set; }

    public int InfluencerId { get; set; }

    public string BankName { get; set; } = null!;

    public string BankAccountNumber { get; set; } = null!;

    public string BankAccountName { get; set; } = null!;

    public virtual FashionInfluencer Influencer { get; set; } = null!;
}
