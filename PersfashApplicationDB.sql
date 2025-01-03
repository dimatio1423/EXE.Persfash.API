--Drop database PersfashApplicationDB
CREATE DATABASE PersfashApplicationDB;
GO
USE PersfashApplicationDB;
GO

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(255) UNIQUE NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FullName NVARCHAR(255),
    Gender VARCHAR(50),
    DateOfBirth DATE,
    ProfilePicture VARCHAR(255),
    DateJoined DATETIME DEFAULT GETDATE(),
	Status VARCHAR(50)
);

CREATE TABLE CustomerProfiles (
    ProfileID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT UNIQUE,
    BodyType NVARCHAR(MAX),
    FashionStyle NVARCHAR(MAX),
	FitPreferences NVARCHAR(MAX),
	PreferredSize NVARCHAR (MAX),
    PreferredColors NVARCHAR(MAX),
    PreferredMaterials NVARCHAR(MAX),
    Occasion NVARCHAR(255),
    Lifestyle NVARCHAR(MAX),
    FacebookLink NVARCHAR(MAX),
    InstagramLink NVARCHAR(MAX),
    TikTokLink NVARCHAR(MAX),
    ProfileSetupComplete BIT,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

/*
CREATE TABLE Partners (
    PartnerID INT PRIMARY KEY IDENTITY(1,1),
    PartnerName NVARCHAR(255),
	PartnerProfilePicture VARCHAR(255),
    WebsiteURL VARCHAR(255),
    ContactEmail VARCHAR(255),
    ContactPhone VARCHAR(255),
	Username VARCHAR(255) UNIQUE,
    Email VARCHAR(255),
	Password VARCHAR(255) NOT NULL,
	Status VARCHAR(50)
);*/

CREATE TABLE FashionItems (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(255) NOT NULL,
    Category VARCHAR(100),
    Brand NVARCHAR(100),
    Price DECIMAL(10, 2),
	FitType NVARCHAR(MAX),
	GenderTarget NVARCHAR(MAX),
	FashionTrend NVARCHAR(MAX),
    Size NVARCHAR(50),
    Color NVARCHAR(MAX),
    Material NVARCHAR(MAX),
	ThumbnailURL NVARCHAR(255),
    Occasion NVARCHAR(MAX),
    ProductURL TEXT,
    DateAdded DATETIME DEFAULT GETDATE(),
	Status VARCHAR(50),
);

CREATE TABLE FashionItem_Images(
   ItemImageID INT PRIMARY KEY IDENTITY(1,1),
   ItemID INT,
   ImageURL NVARCHAR(255),
   FOREIGN KEY (ItemID) REFERENCES FashionItems(ItemID)
)

/*
CREATE TABLE Recommendations (
    RecommendationID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    ItemID INT,
    RecommendationType NVARCHAR(100),
    RecommendationDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ItemID) REFERENCES FashionItems(ItemID)
);*/

CREATE TABLE FashionInfluencers (
    InfluencerID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255),
    Username VARCHAR(255) UNIQUE,
    Email VARCHAR(255) UNIQUE,
	Password VARCHAR(255) NOT NULL,
    Specialty VARCHAR(MAX),
    ProfilePicture VARCHAR(255),
    FacebookLink NVARCHAR(MAX),
    InstagramLink NVARCHAR(MAX),
    TikTokLink NVARCHAR(MAX),
	Status VARCHAR(50),
    DateJoined DATETIME DEFAULT GETDATE()
);

CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(255),
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2),
    InstructorID INT,
	ThumbnailURL VARCHAR(255),
	Status VARCHAR(50),
    FOREIGN KEY (InstructorID) REFERENCES FashionInfluencers(InfluencerID)
);

CREATE TABLE Course_Images(
   CourseImageID INT PRIMARY KEY IDENTITY(1,1),
   CourseID INT,
   ImageURL VARCHAR(255),
   FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
)

CREATE TABLE CourseContent (
    CourseContentID INT PRIMARY KEY IDENTITY(1,1),
    Content NVARCHAR(MAX),
    Duration INT,
	CourseID INT,
	FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

CREATE TABLE CourseMaterial (
    CourseMaterialID INT PRIMARY KEY IDENTITY(1,1),
    MaterialName NVARCHAR(255),
    MaterialLink VARCHAR(255),
	CreatedDate DATETIME DEFAULT GETDATE(),
	CourseContentID INT,
	FOREIGN KEY (CourseContentID) REFERENCES CourseContent(CourseContentID)
);

CREATE TABLE CustomerCourses (
    CustomerCourseID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    CourseID INT,
    EnrollmentDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
);

CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    ItemID INT,
	CourseID INT,
	InfluencerID INT,
    Rating INT CHECK(Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(MAX),
    DateGiven DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
    FOREIGN KEY (InfluencerID) REFERENCES FashionInfluencers(InfluencerID)
);

CREATE TABLE SystemAdmin (
  AdminID INT PRIMARY KEY IDENTITY(1,1),
  Username VARCHAR(255) UNIQUE,
  Password VARCHAR(255) NOT NULL,
  Status VARCHAR(50)
)

CREATE TABLE Wardrobe (
    WardrobeID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    DateAdded DATETIME DEFAULT GETDATE(),
    Notes NVARCHAR(MAX),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE WardrobeItems (
    WardrobeItemID INT PRIMARY KEY IDENTITY(1,1),
    WardrobeID INT,
    ItemID INT,
    FOREIGN KEY (WardrobeID) REFERENCES Wardrobe(WardrobeID),
    FOREIGN KEY (ItemID) REFERENCES FashionItems(ItemID)
);

CREATE TABLE RefreshToken (
   RefreshTokenID INT PRIMARY KEY IDENTITY(1,1),
   ExpiredAt Datetime NOT NULL,
   Token VARCHAR (255) NOT NULL,
   CustomerID INT,
   InfluencerID INT,
   AdminID INT,
   FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
   FOREIGN KEY (InfluencerID) REFERENCES FashionInfluencers(InfluencerID),
   FOREIGN KEY (AdminID) REFERENCES SystemAdmin(AdminID)
);

CREATE TABLE Subscriptions(
	SubscriptionID INT PRIMARY KEY IDENTITY(1,1),
	SubscriptionTitle NVARCHAR(MAX) NOT NULL,
	Price DECIMAL(18, 2),
	DurationInDays INT,
	Description NVARCHAR(MAX),
	Status VARCHAR(50)
)

CREATE TABLE CustomerSubscriptions(
	CustomerSubscriptionID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CustomerID INT NOT NULL,
	SubscriptionID INT NOT NULL,
	StartDate datetime ,
	EndDate datetime ,
	IsActive bit, 
	FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (SubscriptionID) REFERENCES Subscriptions(SubscriptionID)
)

CREATE TABLE Payment(
   PaymentID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
   PaymentDate datetime NOT NULL,
   Price DECIMAL(10, 2) NOT NULL,
   CustomerID INT NOT NULL,
   SubscriptionID INT,
   CourseID INT,
   Status VARCHAR(50),
   FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
   FOREIGN KEY (SubscriptionID) REFERENCES Subscriptions(SubscriptionID),
   FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
)

CREATE TABLE PaymentTransactions (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    PaymentID INT NOT NULL,  
    InfluencerID INT NOT NULL,  
    OriginalAmount DECIMAL(10, 2) NOT NULL,  
	ComissionRate DECIMAL NOT NULL,
    CommissionAmount DECIMAL(10, 2), 
    TransferredAmount DECIMAL(10, 2) NOT NULL, 
    TransferDate DATETIME DEFAULT GETDATE(), 
    Status VARCHAR(50), 
    FOREIGN KEY (PaymentID) REFERENCES Payment(PaymentID),
    FOREIGN KEY (InfluencerID) REFERENCES FashionInfluencers(InfluencerID)
);

CREATE TABLE InfluencerPaymentInformation (
   PaymentInformationID INT PRIMARY KEY IDENTITY (1,1),
   InfluencerID INT NOT NULL,  
   BankName NVARCHAR (MAX) NOT NULL,
   BankAccountNumber NVARCHAR(MAX) NOT NULL,
   BankAccountName NVARCHAR(MAX) NOT NULL,
   FOREIGN KEY (InfluencerID) REFERENCES FashionInfluencers(InfluencerID)
)

CREATE TABLE OutfitCombinations (
    OutfitID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT,
    TopItemID INT,  -- The top fashion item (shirt, blouse, etc.)
    BottomItemID INT,  -- The bottom fashion item (pants, skirt, etc.)
    ShoesItemID INT,  -- Shoes for the outfit
    AccessoriesItemID INT,  -- Any accessories for the outfit
	DressItemID INT, -- For Female customer
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TopItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (BottomItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (ShoesItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (AccessoriesItemID) REFERENCES FashionItems(ItemID),
	FOREIGN KEY (DressItemID) REFERENCES FashionItems(ItemID),
	FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE OutfitFavorite(
    OutfitFavoriteID INT PRIMARY KEY IDENTITY(1,1),
	CustomerID INT,
    TopItemID INT,  -- The top fashion item (shirt, blouse, etc.)
    BottomItemID INT,  -- The bottom fashion item (pants, skirt, etc.)
    ShoesItemID INT,  -- Shoes for the outfit
    AccessoriesItemID INT,  -- Any accessories for the outfit
	DressItemID INT, -- For Female customer
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TopItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (BottomItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (ShoesItemID) REFERENCES FashionItems(ItemID),
    FOREIGN KEY (AccessoriesItemID) REFERENCES FashionItems(ItemID),
	FOREIGN KEY (DressItemID) REFERENCES FashionItems(ItemID),
	FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
)

CREATE TABLE SupportQuestion (
    SupportID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    Question NVARCHAR(255),
    Status VARCHAR(50), -- Open, Answered, Closed
    DateCreated DATETIME DEFAULT GETDATE(),
    DateClosed DATETIME, 
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE SupportMessage (
    MessageID INT PRIMARY KEY IDENTITY(1,1),
    SupportID INT, -- Link to the support thread
    CustomerID INT, -- ID of the customer or admin
	AdminID INT,
    MessageText NVARCHAR(MAX), -- The actual message (question, answer, or reply)
    DateSent DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (AdminID) REFERENCES SystemAdmin(AdminID),
    FOREIGN KEY (SupportID) REFERENCES SupportQuestion(SupportID)
);
