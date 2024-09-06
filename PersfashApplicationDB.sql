
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
    CustomerID INT,
    BodyType VARCHAR(MAX),
    FashionStyle VARCHAR(MAX),
    PreferredColors VARCHAR(MAX),
    PreferredMaterials VARCHAR(MAX),
    Occasion VARCHAR(255),
    Lifestyle TEXT,
    SocialMediaLinks NVARCHAR(MAX),
    ProfileSetupComplete BIT,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE Partners (
    PartnerID INT PRIMARY KEY IDENTITY(1,1),
    PartnerName NVARCHAR(255),
    WebsiteURL VARCHAR(255),
    ContactEmail VARCHAR(255),
    ContactPhone VARCHAR(255),
	Username VARCHAR(255) UNIQUE,
    Email VARCHAR(255),
	Password VARCHAR(255) NOT NULL,
	Status VARCHAR(50)
);

CREATE TABLE FashionItems (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName VARCHAR(255) NOT NULL,
    Category VARCHAR(100),
    Brand VARCHAR(100),
    Price DECIMAL(10, 2),
    Size VARCHAR(50),
    Color VARCHAR(MAX),
    Material VARCHAR(MAX),
    Occasion VARCHAR(MAX),
    ImageURL VARCHAR(255),
    ProductURL VARCHAR(255),
	PartnerID INT,
    DateAdded DATETIME DEFAULT GETDATE(),
	Status VARCHAR(50),
	FOREIGN KEY (PartnerID) REFERENCES Partners(PartnerID)
);

CREATE TABLE Recommendations (
    RecommendationID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    ItemID INT,
    RecommendationType VARCHAR(100),
    RecommendationDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ItemID) REFERENCES FashionItems(ItemID)
);

CREATE TABLE FashionInfluencers (
    InfluencerID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255),
    Username VARCHAR(255) UNIQUE,
    Email VARCHAR(255) UNIQUE,
	Password VARCHAR(255) NOT NULL,
    Specialty VARCHAR(MAX),
    ProfilePicture VARCHAR(255),
    SocialMediaLinks NVARCHAR(MAX),
	Status VARCHAR(50),
    DateJoined DATETIME DEFAULT GETDATE()
);

CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseName VARCHAR(255),
    Description TEXT,
    Price DECIMAL(10, 2),
    InstructorID INT,
	Status VARCHAR(50),
    FOREIGN KEY (InstructorID) REFERENCES FashionInfluencers(InfluencerID)
);

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
    UserCourseID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    CourseID INT,
    EnrollmentDate DATETIME DEFAULT GETDATE(),
    CompletionStatus VARCHAR(50),
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
    Comment TEXT,
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
    Notes TEXT,
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
   PartnerID INT,
   AdminID INT,
   FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
   FOREIGN KEY (PartnerID) REFERENCES Partners(PartnerID),
   FOREIGN KEY (InfluencerID) REFERENCES FashionInfluencers(InfluencerID),
   FOREIGN KEY (AdminID) REFERENCES SystemAdmin(AdminID)
);

CREATE TABLE Subscriptions(
	SubscriptionID INT PRIMARY KEY IDENTITY(1,1),
	SubscriptionTitle NVARCHAR(MAX) NOT NULL,
	Price DECIMAL(18, 2) NOT NULL,
	DurationInDays INT NOT NULL,
	Description NVARCHAR(255) NULL,
	Status VARCHAR(50)
)

CREATE TABLE CustomerSubscriptions(
	CustomerSubscriptionID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CustomerID INT NOT NULL,
	SubscriptionID INT NOT NULL,
	StartDate datetime NOT NULL,
	EndDate datetime NOT NULL,
	IsActive bit NOT NULL, 
	FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (SubscriptionID) REFERENCES Subscriptions(SubscriptionID)
)

CREATE TABLE Payment(
   PaymentID INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
   PayementDate datetime NOT NULL,
   Price DECIMAL(10, 2) NOT NULL,
   CustomerID INT NOT NULL,
   SubscriptionID INT NOT NULL,
   FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
   FOREIGN KEY (SubscriptionID) REFERENCES Subscriptions(SubscriptionID)
)