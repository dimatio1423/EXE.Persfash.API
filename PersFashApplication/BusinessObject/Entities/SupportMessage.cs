using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class SupportMessage
{
    public int MessageId { get; set; }

    public int? SupportId { get; set; }

    public int? CustomerId { get; set; }

    public int? AdminId { get; set; }

    public string? MessageText { get; set; }

    public DateTime? DateSent { get; set; }

    public virtual SystemAdmin? Admin { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual SupportQuestion? Support { get; set; }
}
