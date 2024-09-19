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

    public virtual DbSet<InfluencerPaymentInformation> InfluencerPaymentInformations { get; set; }

    public virtual DbSet<OutfitCombination> OutfitCombinations { get; set; }

    public virtual DbSet<OutfitFavorite> OutfitFavorites { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

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
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71871D9C4530");

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
                .HasConstraintName("FK__Courses__Instruc__49C3F6B7");
        });

        modelBuilder.Entity<CourseContent>(entity =>
        {
            entity.HasKey(e => e.CourseContentId).HasName("PK__CourseCo__91F86B50195DA0B3");

            entity.ToTable("CourseContent");

            entity.Property(e => e.CourseContentId).HasColumnName("CourseContentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseContents)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CourseCon__Cours__4F7CD00D");
        });

        modelBuilder.Entity<CourseImage>(entity =>
        {
            entity.HasKey(e => e.CourseImageId).HasName("PK__Course_I__349B6F849737CB1C");

            entity.ToTable("Course_Images");

            entity.Property(e => e.CourseImageId).HasColumnName("CourseImageID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseImages)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Course_Im__Cours__4CA06362");
        });

        modelBuilder.Entity<CourseMaterial>(entity =>
        {
            entity.HasKey(e => e.CourseMaterialId).HasName("PK__CourseMa__1BC9609B750029CA");

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
                .HasConstraintName("FK__CourseMat__Cours__534D60F1");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8E728EDB7");

            entity.HasIndex(e => e.Username, "UQ__Customer__536C85E4A60C713C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534BE0F65A4").IsUnique();

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
            entity.HasKey(e => e.CustomerCourseId).HasName("PK__Customer__DDCED1331F38599C");

            entity.Property(e => e.CustomerCourseId).HasColumnName("CustomerCourseID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EnrollmentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.CustomerCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__CustomerC__Cours__5812160E");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerCourses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__CustomerC__Custo__571DF1D5");
        });

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__Customer__290C88847F724040");

            entity.HasIndex(e => e.CustomerId, "UQ__Customer__A4AE64B949CC75FB").IsUnique();

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
            entity.HasKey(e => e.CustomerSubscriptionId).HasName("PK__Customer__ADE642676AD64732");

            entity.Property(e => e.CustomerSubscriptionId).HasColumnName("CustomerSubscriptionID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerSubscriptions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerS__Custo__73BA3083");

            entity.HasOne(d => d.Subscription).WithMany(p => p.CustomerSubscriptions)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerS__Subsc__74AE54BC");
        });

        modelBuilder.Entity<FashionInfluencer>(entity =>
        {
            entity.HasKey(e => e.InfluencerId).HasName("PK__FashionI__C32C1162D7A30AD1");

            entity.HasIndex(e => e.Username, "UQ__FashionI__536C85E43C2DDC88").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__FashionI__A9D10534E9960BA0").IsUnique();

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
            entity.HasKey(e => e.ItemId).HasName("PK__FashionI__727E83EBC86AA26A");

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
        });

        modelBuilder.Entity<FashionItemImage>(entity =>
        {
            entity.HasKey(e => e.ItemImageId).HasName("PK__FashionI__09AE32B78ACC9F50");

            entity.ToTable("FashionItem_Images");

            entity.Property(e => e.ItemImageId).HasColumnName("ItemImageID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Item).WithMany(p => p.FashionItemImages)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__FashionIt__ItemI__4222D4EF");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF6F27765BD");

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
                .HasConstraintName("FK__Feedback__Course__5EBF139D");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Feedback__Custom__5CD6CB2B");

            entity.HasOne(d => d.Influencer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.InfluencerId)
                .HasConstraintName("FK__Feedback__Influe__5FB337D6");

            entity.HasOne(d => d.Item).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Feedback__ItemID__5DCAEF64");
        });

        modelBuilder.Entity<InfluencerPaymentInformation>(entity =>
        {
            entity.HasKey(e => e.PaymentInformationId).HasName("PK__Influenc__EF3284597FB57092");

            entity.ToTable("InfluencerPaymentInformation");

            entity.Property(e => e.PaymentInformationId).HasColumnName("PaymentInformationID");
            entity.Property(e => e.BankAccountNumber).IsUnicode(false);
            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");

            entity.HasOne(d => d.Influencer).WithMany(p => p.InfluencerPaymentInformations)
                .HasForeignKey(d => d.InfluencerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Influence__Influ__01142BA1");
        });

        modelBuilder.Entity<OutfitCombination>(entity =>
        {
            entity.HasKey(e => e.OutfitId).HasName("PK__OutfitCo__399B99D16291A7ED");

            entity.Property(e => e.OutfitId).HasColumnName("OutfitID");
            entity.Property(e => e.AccessoriesItemId).HasColumnName("AccessoriesItemID");
            entity.Property(e => e.BottomItemId).HasColumnName("BottomItemID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DressItemId).HasColumnName("DressItemID");
            entity.Property(e => e.ShoesItemId).HasColumnName("ShoesItemID");
            entity.Property(e => e.TopItemId).HasColumnName("TopItemID");

            entity.HasOne(d => d.AccessoriesItem).WithMany(p => p.OutfitCombinationAccessoriesItems)
                .HasForeignKey(d => d.AccessoriesItemId)
                .HasConstraintName("FK__OutfitCom__Acces__07C12930");

            entity.HasOne(d => d.BottomItem).WithMany(p => p.OutfitCombinationBottomItems)
                .HasForeignKey(d => d.BottomItemId)
                .HasConstraintName("FK__OutfitCom__Botto__05D8E0BE");

            entity.HasOne(d => d.Customer).WithMany(p => p.OutfitCombinations)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__OutfitCom__Custo__09A971A2");

            entity.HasOne(d => d.DressItem).WithMany(p => p.OutfitCombinationDressItems)
                .HasForeignKey(d => d.DressItemId)
                .HasConstraintName("FK__OutfitCom__Dress__08B54D69");

            entity.HasOne(d => d.ShoesItem).WithMany(p => p.OutfitCombinationShoesItems)
                .HasForeignKey(d => d.ShoesItemId)
                .HasConstraintName("FK__OutfitCom__Shoes__06CD04F7");

            entity.HasOne(d => d.TopItem).WithMany(p => p.OutfitCombinationTopItems)
                .HasForeignKey(d => d.TopItemId)
                .HasConstraintName("FK__OutfitCom__TopIt__04E4BC85");
        });

        modelBuilder.Entity<OutfitFavorite>(entity =>
        {
            entity.HasKey(e => e.OutfitFavoriteId).HasName("PK__OutfitFa__4395BEE2E5D36487");

            entity.ToTable("OutfitFavorite");

            entity.Property(e => e.OutfitFavoriteId).HasColumnName("OutfitFavoriteID");
            entity.Property(e => e.AccessoriesItemId).HasColumnName("AccessoriesItemID");
            entity.Property(e => e.BottomItemId).HasColumnName("BottomItemID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DressItemId).HasColumnName("DressItemID");
            entity.Property(e => e.ShoesItemId).HasColumnName("ShoesItemID");
            entity.Property(e => e.TopItemId).HasColumnName("TopItemID");

            entity.HasOne(d => d.AccessoriesItem).WithMany(p => p.OutfitFavoriteAccessoriesItems)
                .HasForeignKey(d => d.AccessoriesItemId)
                .HasConstraintName("FK__OutfitFav__Acces__10566F31");

            entity.HasOne(d => d.BottomItem).WithMany(p => p.OutfitFavoriteBottomItems)
                .HasForeignKey(d => d.BottomItemId)
                .HasConstraintName("FK__OutfitFav__Botto__0E6E26BF");

            entity.HasOne(d => d.Customer).WithMany(p => p.OutfitFavorites)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__OutfitFav__Custo__123EB7A3");

            entity.HasOne(d => d.DressItem).WithMany(p => p.OutfitFavoriteDressItems)
                .HasForeignKey(d => d.DressItemId)
                .HasConstraintName("FK__OutfitFav__Dress__114A936A");

            entity.HasOne(d => d.ShoesItem).WithMany(p => p.OutfitFavoriteShoesItems)
                .HasForeignKey(d => d.ShoesItemId)
                .HasConstraintName("FK__OutfitFav__Shoes__0F624AF8");

            entity.HasOne(d => d.TopItem).WithMany(p => p.OutfitFavoriteTopItems)
                .HasForeignKey(d => d.TopItemId)
                .HasConstraintName("FK__OutfitFav__TopIt__0D7A0286");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58C3A7D178");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");

            entity.HasOne(d => d.Course).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Payment__CourseI__797309D9");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Custome__778AC167");

            entity.HasOne(d => d.Subscription).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SubscriptionId)
                .HasConstraintName("FK__Payment__Subscri__787EE5A0");
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__PaymentT__55433A4B40D67758");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.ComissionRate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CommissionAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");
            entity.Property(e => e.OriginalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransferDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransferredAmount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Influencer).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.InfluencerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentTr__Influ__7E37BEF6");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentTr__Payme__7D439ABD");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E594BE6906B");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.InfluencerId).HasColumnName("InfluencerID");
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Admin).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__RefreshTo__Admin__6EF57B66");

            entity.HasOne(d => d.Customer).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__RefreshTo__Custo__6D0D32F4");

            entity.HasOne(d => d.Influencer).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.InfluencerId)
                .HasConstraintName("FK__RefreshTo__Influ__6E01572D");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B24BD2E0988B4");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SystemAdmin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__SystemAd__719FE4E821EED2FB");

            entity.ToTable("SystemAdmin");

            entity.HasIndex(e => e.Username, "UQ__SystemAd__536C85E45C851A52").IsUnique();

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
            entity.HasKey(e => e.WardrobeId).HasName("PK__Wardrobe__D1E4D2E299A9385F");

            entity.ToTable("Wardrobe");

            entity.Property(e => e.WardrobeId).HasColumnName("WardrobeID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasColumnType("text");

            entity.HasOne(d => d.Customer).WithMany(p => p.Wardrobes)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Wardrobe__Custom__66603565");
        });

        modelBuilder.Entity<WardrobeItem>(entity =>
        {
            entity.HasKey(e => e.WardrobeItemId).HasName("PK__Wardrobe__72A363C93EF56885");

            entity.Property(e => e.WardrobeItemId).HasColumnName("WardrobeItemID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.WardrobeId).HasColumnName("WardrobeID");

            entity.HasOne(d => d.Item).WithMany(p => p.WardrobeItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__WardrobeI__ItemI__6A30C649");

            entity.HasOne(d => d.Wardrobe).WithMany(p => p.WardrobeItems)
                .HasForeignKey(d => d.WardrobeId)
                .HasConstraintName("FK__WardrobeI__Wardr__693CA210");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
