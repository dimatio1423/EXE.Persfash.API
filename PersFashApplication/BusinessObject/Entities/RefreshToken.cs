﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public DateTime ExpiredAt { get; set; }

    public string Token { get; set; } = null!;

    public int? CustomerId { get; set; }

    public int? InfluencerId { get; set; }

    public int? PartnerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual FashionInfluencer? Influencer { get; set; }

    public virtual Partner? Partner { get; set; }
}
