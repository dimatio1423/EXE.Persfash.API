using System;
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

    public virtual DbSet<CourseMaterial> CourseMaterials { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerCourse> CustomerCourses { get; set; }

    public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }

    public virtual DbSet<CustomerSubscription> CustomerSubscriptions { get; set; }

    public virtual DbSet<FashionInfluencer> FashionInfluencers { get; set; }

    public virtual DbSet<FashionItem> FashionItems { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

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
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D718787E63387");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.InstructorId).HasColumnName("InstructorID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Courses__Instruc__4E88ABD4");
        });

        modelBuilder.Entity<CourseContent>(entity =>
        {
            entity.HasKey(e => e.CourseContentId).HasName("PK__CourseCo__91F86B50902192C3");

            entity.ToTable("CourseContent");

            entity.Property(e => e.CourseContentId).HasColumnName("CourseContentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseContents)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CourseCon__Cours__5165187F");
        });

        modelBuilder.Entity<CourseMaterial>(entity =>
        {
            entity.HasKey(e => e.CourseMaterialId).HasName("PK__CourseMa__1BC9609BB0261234");

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
                .HasConstraintName("FK__CourseMat__Cours__5535A963");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8A85E591C");

            entity.HasIndex(e => e.Username, "UQ__Customer__536C85E4FE11BCD2").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105340AC0C94E").IsUnique();

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
            entity.HasKey(e => e.UserCourseId).HasName("PK__Customer__58886EF4903A14BC");

            entity.Property(e => e.UserCourseId).HasColumnName("UserCourseID");
            entity.Property(e => e.CompletionStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CustomerCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CustomerC__Cours__59FA5E80");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerCourses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerC__Custo__59063A47");
        });

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Customer__290C8884DD489712");

            entity.Property(e => e.ProfileId).HasColumnName("ProfileID");
            entity.Property(e => e.BodyType).IsUnicode(false);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FashionStyle).IsUnicode(false);
            entity.Property(e => e.Lifestyle).HasColumnType("text");
            entity.Property(e => e.Occasion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PreferredColors).IsUnicode(false);
            entity.Property(e => e.PreferredMaterials).IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerProfiles)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerP__Custo__3B75D760");
        });

        modelBuilder.Entity<CustomerSubscription>(entity =>
        {
            entity.HasKey(e => e.CustomerSubscriptionId).HasName("PK__Customer__ADE6426707260BCB");

            entity.Property(e => e.CustomerSubscriptionId).HasColumnName("CustomerSubscriptionID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerSubscriptions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerS__Custo__6B24EA82");

            entity.HasOne(d => d.Subscription).WithMany(p => p.CustomerSubscriptions)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerS__Subsc__6C190EBB");
        });

        modelBuilder.Entity<FashionInfluencer>(entity =>
        {
            entity.HasKey(e => e.InfluencerId).HasName("PK__FashionI__C32C1162CC32B369");

            entity.HasIndex(e => e.Username, "UQ__FashionI__536C85E40F4DC297").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__FashionI__A9D1053416AF47FE").IsUnique();

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
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FashionItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__FashionI__727E83EB9BE759AD");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Color).IsUnicode(false);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
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

            entity.HasOne(d => d.Partner).WithMany(p => p.FashionItems)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__FashionIt__Partn__4222D4EF");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF69A6722F9");

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
                .HasConstraintName("FK__Feedback__Course__60A75C0F");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Feedback__Custom__5EBF139D");

            entity.HasOne(d => d.Influencer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.InfluencerId)
                .HasConstraintName("FK__Feedback__Influe__619B8048");

            entity.HasOne(d => d.Item).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Feedback__ItemID__5FB337D6");
        });

        modelBuilder.Entity<Partner>(entity =>
        {
            entity.HasKey(e => e.PartnerId).HasName("PK__Partners__39FD63320BB6763D");

            entity.HasIndex(e => e.Username, "UQ__Partners__536C85E4EA401965").IsUnique();

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
            entity.Property(e => e.Password)
                .HasMaxLength(255)
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
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A583448515A");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PayementDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Custome__6EF57B66");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Subscri__6FE99F9F");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.RecommendationId).HasName("PK__Recommen__AA15BEC491DB2611");

            entity.Property(e => e.RecommendationId).HasColumnName("RecommendationID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.RecommendationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RecommendationType)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Recommend__Custo__45F365D3");

            entity.HasOne(d => d.Item).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Recommend__ItemI__46E78A0C");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E59BDEEB280");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");
            entity.Property(e => e.PartnerId).HasColumnName("PartnerID");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__RefreshTo__Custo__6477ECF3");

            entity.HasOne(d => d.Influencer).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.InfluencerId)
                .HasConstraintName("FK__RefreshTo__Influ__66603565");

            entity.HasOne(d => d.Partner).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.PartnerId)
                .HasConstraintName("FK__RefreshTo__Partn__656C112C");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B24BDD212B622");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
