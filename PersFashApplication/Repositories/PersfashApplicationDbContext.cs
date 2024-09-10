﻿using System;
using System.Collections.Generic;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repositories;

public partial class PersfashApplicationDbContext : DbContext
{
    public PersfashApplicationDbContext()
    {
    }

    public PersfashApplicationDbContext(DbContextOptions<PersfashApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseContent> CourseContents { get; set; }

    public virtual DbSet<CourseImage> CourseImages { get; set; }

    public virtual DbSet<CourseMaterial> CourseMaterials { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerCourse> CustomerCourses { get; set; }

    public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }

    public virtual DbSet<CustomerSubscription> CustomerSubscriptions { get; set; }

    public virtual DbSet<FashionInfluencer> FashionInfluencers { get; set; }

    public virtual DbSet<FashionItem> FashionItems { get; set; }

    public virtual DbSet<FashionItemImage> FashionItemImages { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SystemAdmin> SystemAdmins { get; set; }

    public virtual DbSet<Wardrobe> Wardrobes { get; set; }

    public virtual DbSet<WardrobeItem> WardrobeItems { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187D7ED71BB");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ThumbnailURL");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Courses__Instruc__52593CB8");
        });

        modelBuilder.Entity<CourseContent>(entity =>
        {
            entity.HasKey(e => e.CourseContentId).HasName("PK__CourseCo__91F86B5082C1F583");

            entity.ToTable("CourseContent");

            entity.Property(e => e.CourseContentId).HasColumnName("CourseContentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseContents)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CourseCon__Cours__5812160E");
        });

        modelBuilder.Entity<CourseImage>(entity =>
        {
            entity.HasKey(e => e.CourseImageId).HasName("PK__Course_I__349B6F848A5E9695");

            entity.ToTable("Course_Images");

            entity.Property(e => e.CourseImageId).HasColumnName("CourseImageID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseImages)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Course_Im__Image__5535A963");
        });

        modelBuilder.Entity<CourseMaterial>(entity =>
        {
            entity.HasKey(e => e.CourseMaterialId).HasName("PK__CourseMa__1BC9609B8C86F360");

            entity.ToTable("CourseMaterial");

            entity.Property(e => e.CourseMaterialId).HasColumnName("CourseMaterialID");
            entity.Property(e => e.CourseContentId).HasColumnName("CourseContentID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaterialLink)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MaterialName).HasMaxLength(255);

            entity.HasOne(d => d.CourseContent).WithMany(p => p.CourseMaterials)
                .HasForeignKey(d => d.CourseContentId)
                .HasConstraintName("FK__CourseMat__Cours__5BE2A6F2");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B830D9FAB8");

            entity.HasIndex(e => e.Username, "UQ__Customer__536C85E417A47299").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105344063E7E5").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateJoined)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CustomerCourse>(entity =>
        {
            entity.HasKey(e => e.CustomerCourseId).HasName("PK__Customer__DDCED133809241A9");

            entity.Property(e => e.CustomerCourseId).HasColumnName("CustomerCourseID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CustomerCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CustomerC__Cours__60A75C0F");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerCourses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerC__Custo__5FB337D6");
        });

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Customer__290C8884CC081384");

            entity.HasIndex(e => e.CustomerId, "UQ__Customer__A4AE64B9B3FE0F57").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.BodyType).IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FashionStyle).IsUnicode(false);
            entity.Property(e => e.FitPreferences).IsUnicode(false);
            entity.Property(e => e.Lifestyle).HasColumnType("text");
            entity.Property(e => e.Occasion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PreferredColors).IsUnicode(false);
            entity.Property(e => e.PreferredMaterials).IsUnicode(false);
            entity.Property(e => e.PreferredSize).IsUnicode(false);

            entity.HasOne(d => d.Customer).WithOne(p => p.CustomerProfile)
                .HasForeignKey<CustomerProfile>(d => d.CustomerId)
                .HasConstraintName("FK__CustomerP__Custo__3C69FB99");
        });

        modelBuilder.Entity<CustomerSubscription>(entity =>
        {
            entity.HasKey(e => e.CustomerSubscriptionId).HasName("PK__Customer__ADE64267AC30369D");

            entity.Property(e => e.CustomerSubscriptionId).HasColumnName("CustomerSubscriptionID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerSubscriptions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerS__Custo__7D439ABD");

            entity.HasOne(d => d.Subscription).WithMany(p => p.CustomerSubscriptions)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerS__Subsc__7E37BEF6");
        });

        modelBuilder.Entity<FashionInfluencer>(entity =>
        {
            entity.HasKey(e => e.InfluencerId).HasName("PK__FashionI__C32C11622D7C556A");

            entity.HasIndex(e => e.Username, "UQ__FashionI__536C85E41675E717").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__FashionI__A9D10534C5D633A5").IsUnique();

            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");
            entity.Property(e => e.DateJoined)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Specialty).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FashionItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__FashionI__727E83EB17987E78");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Color).IsUnicode(false);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FashionTrend).IsUnicode(false);
            entity.Property(e => e.FitType).IsUnicode(false);
            entity.Property(e => e.GenderTarget).IsUnicode(false);
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.Material).IsUnicode(false);
            entity.Property(e => e.Occasion).IsUnicode(false);
            entity.Property(e => e.PartnerId).HasColumnName("PartnerID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ProductURL");
            entity.Property(e => e.Size)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ThumbnailURL");

            entity.HasOne(d => d.Partner).WithMany(p => p.FashionItems)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__FashionIt__Partn__4316F928");
        });

        modelBuilder.Entity<FashionItemImage>(entity =>
        {
            entity.HasKey(e => e.ItemImageId).HasName("PK__FashionI__09AE32B77FE79055");

            entity.ToTable("FashionItem_Images");

            entity.Property(e => e.ItemImageId).HasColumnName("ItemImageID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Item).WithMany(p => p.FashionItemImages)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__FashionIt__Image__45F365D3");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF66F97132E");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateGiven)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Course).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Feedback__Course__6754599E");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Feedback__Custom__656C112C");

            entity.HasOne(d => d.Influencer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.InfluencerId)
                .HasConstraintName("FK__Feedback__Influe__68487DD7");

            entity.HasOne(d => d.Item).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Feedback__ItemID__66603565");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__Partners__39FD63327269E101");

            entity.HasIndex(e => e.Username, "UQ__Partners__536C85E475F97727").IsUnique();

            entity.Property(e => e.PartnerId).HasColumnName("PartnerID");
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PartnerName).HasMaxLength(255);
            entity.Property(e => e.PartnerProfilePicture)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("WebsiteURL");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A5808A32610");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PayementDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Course).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Payment__CourseI__02FC7413");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Custome__01142BA1");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK__Payment__Subscri__02084FDA");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.RecommendationId).HasName("PK__Recommen__AA15BEC4B5A92A63");

            entity.Property(e => e.RecommendationId).HasColumnName("RecommendationID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.RecommendationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RecommendationType).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Recommend__Custo__49C3F6B7");

            entity.HasOne(d => d.Item).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Recommend__ItemI__4AB81AF0");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E59E2843D64");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");
            entity.Property(e => e.PartnerId).HasColumnName("PartnerID");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Admin).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__RefreshTo__Admin__787EE5A0");

            entity.HasOne(d => d.Customer).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__RefreshTo__Custo__75A278F5");

            entity.HasOne(d => d.Influencer).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.InfluencerId)
                .HasConstraintName("FK__RefreshTo__Influ__778AC167");

            entity.HasOne(d => d.Partner).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__RefreshTo__Partn__76969D2E");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B24BD7DDC729F");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SystemAdmin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__SystemAd__719FE4E81002A3D5");

            entity.ToTable("SystemAdmin");

            entity.HasIndex(e => e.Username, "UQ__SystemAd__536C85E4C4F96110").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Wardrobe>(entity =>
        {
            entity.HasKey(e => e.WardrobeId).HasName("PK__Wardrobe__D1E4D2E21E4A0977");

            entity.ToTable("Wardrobe");

            entity.Property(e => e.WardrobeId).HasColumnName("WardrobeID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasColumnType("text");

            entity.HasOne(d => d.Customer).WithMany(p => p.Wardrobes)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Wardrobe__Custom__6EF57B66");
        });

        modelBuilder.Entity<WardrobeItem>(entity =>
        {
            entity.HasKey(e => e.WardrobeItemId).HasName("PK__Wardrobe__72A363C96B18EC1B");

            entity.Property(e => e.WardrobeItemId).HasColumnName("WardrobeItemID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.WardrobeId).HasColumnName("WardrobeID");

            entity.HasOne(d => d.Item).WithMany(p => p.WardrobeItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__WardrobeI__ItemI__72C60C4A");

            entity.HasOne(d => d.Wardrobe).WithMany(p => p.WardrobeItems)
                .HasForeignKey(d => d.WardrobeId)
                .HasConstraintName("FK__WardrobeI__Wardr__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}