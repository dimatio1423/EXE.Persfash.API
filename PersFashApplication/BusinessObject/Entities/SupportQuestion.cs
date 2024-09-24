using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class SupportQuestion
{
    public int SupportId { get; set; }

    public int? CustomerId { get; set; }

    public string? Question { get; set; }

    public string? Status { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateClosed { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<SupportMessage> SupportMessages { get; set; } = new List<SupportMessage>();
}
