using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class FashionInfluencer
{
    public int InfluencerId { get; set; }

    public string? FullName { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public string? Specialty { get; set; }

    public string? ProfilePicture { get; set; }

    public string? SocialMediaLinks { get; set; }

    public DateTime? DateJoined { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
