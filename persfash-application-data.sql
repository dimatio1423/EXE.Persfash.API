INSERT INTO Customers(Username, Email, Password, FullName, Gender, DateOfBirth, ProfilePicture, Status)
VALUES
('johndoe', 'johndoe@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'John Doe', 'Male', '1990-05-15', 'johndoe.jpg', 'Active'),
('janedoe', 'janedoe@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Jane Doe', 'Female', '1992-08-25', 'janedoe.jpg', 'Active'),
('michael', 'michael@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Michael Smith', 'Male', '1988-03-22', 'michael.jpg', 'Inactive'),
('emily', 'emily@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Emily Johnson', 'Female', '1995-11-05', 'emily.jpg', 'Active');


INSERT INTO CustomerProfiles (CustomerID, BodyType, FashionStyle, FitPreferences, PreferredSize, PreferredColors, PreferredMaterials, Occasion, Lifestyle, FacebookLink, ProfileSetupComplete)
VALUES
(1, 'Rectangle', 'Casual', 'Slim', 'M,L', 'Blue, Black', 'Cotton, Denim', 'Casual, Business', 'Outdoor enthusiast, Tech-savvy', 'http://twitter.com/johndoe', 1),
(2, 'Hourglass', 'Chic', 'Regular', 'M,L', 'Red, White', 'Silk, Leather', 'Formal, Party', 'Fashion blogger, Travel lover', 'http://instagram.com/janedoe', 1),
(3, 'Inverted Triangle', 'Athleisure','Loose', 'L, XL', 'Gray, Green', 'Polyester, Nylon', 'Sport, Casual', 'Gym lover, Adventure seeker', 'http://linkedin.com/in/michael', 1),
(4, 'Pear', 'Bohemian', 'Regular', 'M' , 'Purple, Yellow', 'Linen, Cotton', 'Vacation, Wedding', 'Art enthusiast, Yoga practitioner', 'http://facebook.com/emilyjohnson', 1);


INSERT INTO Partners (PartnerName, PartnerProfilePicture, WebsiteURL, ContactEmail, ContactPhone, Email, Password, Username, Status)
VALUES
('FashionWorld', 'profileURL.png','http://fashionworld.com', 'contact@fashionworld.com', '123-456-7890', 'partner1@fashionworld.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'partner1', 'Active'),
('StyleHub', 'profileURL2.png','http://stylehub.com', 'support@stylehub.com', '098-765-4321', 'partner2@stylehub.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'partner2', 'Active');


INSERT INTO FashionItems (ItemName, Category, Brand, Price, FitType, GenderTarget, FashionTrend, Size, Color, Material, Occasion, ThumbnailURL, ProductURL, PartnerID, Status)
VALUES
('Slim Fit Blazer', 'Jacket', 'Zara', 150.00, 'Slim', 'Unisex', 'Minimalist' ,'M', 'Gray', 'Wool', 'Business', 'slimfitblazer.jpg', 'www.fashionco.com/slimfitblazer', 1, 'Available'),
('Summer Dress', 'Dress', 'H&M', 75.00, 'Regular', 'Women', 'Vintage' ,'S', 'Red', 'Cotton', 'Casual', 'summerdress.jpg', 'www.styleworld.com/summerdress', 2, 'Available');


INSERT INTO Recommendations (CustomerID, ItemID, RecommendationType)
VALUES
(1, 1, 'Personalized'),
(2, 2, 'Trending');


INSERT INTO FashionInfluencers (FullName, Username, Email, Password, Specialty, ProfilePicture, FacebookLink, Status)
VALUES
('Sophia Lee', 'sophialee', 'sophia@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Chic & Minimalist Fashion', 'sophia.jpg', 'http://instagram.com/sophialee', 'Active'),
('James Kim', 'jameskim', 'james@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Streetwear & Casual Fashion', 'james.jpg', 'http://twitter.com/jameskim', 'Active');

INSERT INTO Courses (CourseName, Description, Price, InstructorID, ThumbnailURL, Status)
VALUES
('Mastering Streetwear', 'A comprehensive course on streetwear fashion.', 49.99, 2,'thumbnail.jpg', 'Available'),
('Chic Fashion Basics', 'Learn the basics of chic fashion style.', 39.99, 1, 'thumbnail.jpg', 'Available');

INSERT INTO CourseContent (Content, Duration, CourseID)
VALUES
('Introduction to Streetwear', 15, 1),
('Streetwear Outfit Combinations', 30, 1),
('Chic Fashion Essentials', 20, 2),
('Building a Chic Wardrobe', 25, 2);

INSERT INTO CourseMaterial (MaterialName, MaterialLink, CourseContentID)
VALUES
('Streetwear Lookbook', 'http://fashionworld.com/lookbook.pdf', 1),
('Chic Wardrobe Guide', 'http://stylehub.com/wardrobe_guide.pdf', 3);

INSERT INTO CustomerCourses (CustomerID, CourseID, EnrollmentDate)
VALUES
(1, 1, '2024-08-01'),
(2, 2, '2024-07-15'),
(3, 1, '2024-08-20'),
(4, 2, '2024-08-05');

INSERT INTO Feedback (CustomerID, ItemID, CourseID, InfluencerID, Rating, Comment)
VALUES
(1, 1, NULL, NULL, 4, 'Great quality denim jacket!'),
(2, NULL, 2, 1, 5, 'Very informative and well-structured course!'),
(4, NULL, 1, 2, 5, 'Loved the streetwear course, learned a lot!');

INSERT INTO Subscriptions (SubscriptionTitle, Price, DurationInDays, Description, Status)
VALUES
('Free', null, null, 'Access to basic features and limited courses.', 'Active'),
('Premium', 19.99, 60, 'Access to all features and unlimited courses.', 'Active'),
('Courses', 29.99, 90, 'All-access pass including exclusive content and perks.', 'Active');

INSERT INTO SystemAdmin (Username, Password, Status)
VALUES
('admin', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Active');

INSERT INTO Wardrobe (CustomerID, Notes)
VALUES
(1, 'My summer wardrobe collection'),
(2, 'Formal wear wardrobe');

-- Sample data for WardrobeItems table
INSERT INTO WardrobeItems (WardrobeID, ItemID)
VALUES 
(1, 1),
(2, 2);

INSERT INTO CustomerSubscriptions (CustomerID, SubscriptionID, StartDate, EndDate, IsActive)
VALUES
(1, 1, null, null, 1),  
(2, 2, '2024-08-15', '2024-11-15', 1), 
(3, 3, '2024-06-01', '2024-08-30', 0);  

