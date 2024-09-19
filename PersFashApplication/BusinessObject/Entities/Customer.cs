using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? ProfilePicture { get; set; }

    public DateTime? DateJoined { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<CustomerCourse> CustomerCourses { get; set; } = new List<CustomerCourse>();

    public virtual CustomerProfile? CustomerProfile { get; set; }

    public virtual ICollection<CustomerSubscription> CustomerSubscriptions { get; set; } = new List<CustomerSubscription>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<OutfitCombination> OutfitCombinations { get; set; } = new List<OutfitCombination>();

    public virtual ICollection<OutfitFavorite> OutfitFavorites { get; set; } = new List<OutfitFavorite>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<Wardrobe> Wardrobes { get; set; } = new List<Wardrobe>();
}
