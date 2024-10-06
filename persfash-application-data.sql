INSERT INTO Customers(Username, Email, Password, FullName, Gender, DateOfBirth, ProfilePicture, Status)
VALUES
('johndoe', 'johndoe@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'John Doe', 'Male', '1990-05-15', 'johndoe.jpg', 'Active'),
('janedoe', 'janedoe@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Jane Doe', 'Female', '1992-08-25', 'janedoe.jpg', 'Active'),
('michael', 'michael@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Michael Smith', 'Male', '1988-03-22', 'michael.jpg', 'Inactive'),
('emily', 'emily@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Emily Johnson', 'Female', '1995-11-05', 'emily.jpg', 'Active');


INSERT INTO CustomerProfiles (CustomerID, BodyType, FashionStyle, FitPreferences, PreferredSize, PreferredColors, PreferredMaterials, Occasion, Lifestyle, FacebookLink, ProfileSetupComplete)
VALUES
(1, 'Rectangle', 'Casual', 'Slim', 'M, L', 'Blue, Black', 'Cotton, Denim', 'Casual, Business', 'Outdoor enthusiast, Tech-savvy', 'http://twitter.com/johndoe', 1),
(2, 'Hourglass', 'Chic', 'Regular', 'M, L', 'Red, White', 'Silk, Leather', 'Formal, Party', 'Fashion blogger, Travel lover', 'http://instagram.com/janedoe', 1),
(3, 'Inverted Triangle', 'Athleisure','Loose', 'L, XL', 'Gray, Green', 'Polyester, Nylon', 'Sport, Casual', 'Gym lover, Adventure seeker', 'http://linkedin.com/in/michael', 1),
(4, 'Pear', 'Bohemian', 'Regular', 'M' , 'Purple, Yellow', 'Linen, Cotton', 'Vacation, Wedding', 'Art enthusiast, Yoga practitioner', 'http://facebook.com/emilyjohnson', 1);

/*
INSERT INTO Partners (PartnerName, PartnerProfilePicture, WebsiteURL, ContactEmail, ContactPhone, Email, Password, Username, Status)
VALUES
('FashionWorld', 'profileURL.png','http://fashionworld.com', 'contact@fashionworld.com', '123-456-7890', 'partner1@fashionworld.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'partner1', 'Active'),
('StyleHub', 'profileURL2.png','http://stylehub.com', 'support@stylehub.com', '098-765-4321', 'partner2@stylehub.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'partner2', 'Active');
*/

INSERT INTO FashionItems (ItemName, Category, Brand, Price, FitType, GenderTarget, FashionTrend, Size, Color, Material, Occasion, ThumbnailURL, ProductURL, Status)
VALUES
('Slim Fit Blazer', 'Tops', 'Zara', 150.00, 'Slim', 'Unisex', 'Minimalist' ,'M', 'Gray', 'Wool', 'Business', 'slimfitblazer.jpg', 'www.fashionco.com/slimfitblazer', 'Available'),
('Summer Dress', 'Dresses', 'H&M', 75.00, 'Regular', 'Women', 'Vintage' ,'S', 'Red', 'Cotton', 'Casual', 'summerdress.jpg', 'www.styleworld.com/summerdress', 'Available'),
-- Tops
('V-Neck T-Shirt', 'Tops', 'H&M', 20.00, 'Regular', 'Unisex', 'Casual', 'M', 'White', 'Cotton', 'Casual', 'vneck_tshirt.jpg', 'www.hm.com/vnecktshirt', 'Available'),
('Button-Down Blouse', 'Tops', 'Zara', 55.00, 'Regular', 'Women', 'Chic', 'S', 'Blue', 'Silk', 'Business', 'buttondown_blouse.jpg', 'www.zara.com/buttondownblouse', 'Available'),

-- Bottoms
('Straight-Leg Jeans', 'Bottoms', 'Levis', 80.00, 'Regular', 'Men', 'Classic', 'L', 'Dark Blue', 'Denim', 'Casual', 'straightleg_jeans.jpg', 'www.levis.com/straightlegjeans', 'Available'),
('Pleated Skirt', 'Bottoms', 'H&M', 45.00, 'Regular', 'Women', 'Preppy', 'M', 'Navy', 'Cotton', 'Formal', 'pleated_skirt.jpg', 'www.hm.com/pleatedskirt', 'Available'),

-- Dresses
('Cocktail Dress', 'Dresses', 'Forever 21', 90.00, 'Slim', 'Women', 'Glam', 'S', 'Black', 'Polyester', 'Party', 'cocktail_dress.jpg', 'www.forever21.com/cocktaildress', 'Available'),
('Maxi Sundress', 'Dresses', 'Mango', 60.00, 'Regular', 'Women', 'Bohemian', 'M', 'Floral', 'Linen', 'Vacation', 'maxi_sundress.jpg', 'www.mango.com/maxisundress', 'Available'),

-- Shoes
('Running Shoes', 'Shoes', 'Adidas', 120.00, 'Regular', 'Unisex', 'Athleisure', '9', 'Gray', 'Mesh', 'Sport', 'running_shoes.jpg', 'www.adidas.com/runningshoes', 'Available'),
('Ankle Boots', 'Shoes', 'Aldo', 130.00, 'Slim', 'Women', 'Chic', '7', 'Brown', 'Leather', 'Casual', 'ankle_boots.jpg', 'www.aldo.com/ankleboots', 'Available'),

-- Accessories
('Leather Crossbody Bag', 'Accessories', 'Coach', 250.00, 'Regular', 'Women', 'Luxury', 'L', 'Black', 'Leather', 'Formal', 'crossbody_bag.jpg', 'www.coach.com/crossbodybag', 'Available'),
('Baseball Cap', 'Accessories', 'Nike', 30.00, 'Regular', 'Unisex', 'Sporty', 'One Size', 'Red', 'Cotton', 'Sport', 'baseball_cap.jpg', 'www.nike.com/baseballcap', 'Available');

INSERT INTO FashionItems (ItemName, Category, Brand, Price, FitType, GenderTarget, FashionTrend, Size, Color, Material, Occasion, ThumbnailURL, ProductURL, Status)
VALUES
('Crew Neck T-Shirt', 'Tops', 'Uniqlo', 25.00, 'Regular', 'Men', 'Casual', 'L', 'Navy', 'Cotton', 'Casual', 'crew_neck_tshirt.jpg', 'www.uniqlo.com/crewnecktshirt', 'Available'),
('Polo Shirt', 'Tops', 'Lacoste', 80.00, 'Slim', 'Men', 'Classic', 'M', 'White', 'Cotton', 'Formal', 'polo_shirt.jpg', 'www.lacoste.com/poloshirt', 'Available'),
('Denim Jacket', 'Tops', 'Levis', 110.00, 'Regular', 'Men', 'Classic', 'L', 'Blue', 'Denim', 'Casual', 'denim_jacket.jpg', 'www.levis.com/denimjacket', 'Available'),
('Formal Shirt', 'Tops', 'Tommy Hilfiger', 65.00, 'Slim', 'Men', 'Business', 'L', 'Blue', 'Cotton', 'Business', 'formal_shirt.jpg', 'www.tommy.com/formalshirt', 'Available'),
('Graphic Hoodie', 'Tops', 'Nike', 60.00, 'Regular', 'Men', 'Casual', 'XL', 'Black', 'Polyester', 'Casual', 'graphic_hoodie.jpg', 'www.nike.com/graphichoodie', 'Available'),
-- Tops for Women
('Crop Top', 'Tops', 'Forever 21', 20.00, 'Slim', 'Women', 'Sporty', 'S', 'Pink', 'Cotton', 'Casual', 'crop_top.jpg', 'www.forever21.com/croptop', 'Available'),
('Peplum Blouse', 'Tops', 'H&M', 40.00, 'Regular', 'Women', 'Luxury', 'M', 'White', 'Silk', 'Formal', 'peplum_blouse.jpg', 'www.hm.com/peplumblouse', 'Available'),
('Off-Shoulder Top', 'Tops', 'Zara', 35.00, 'Slim', 'Women', 'Luxury', 'S', 'Yellow', 'Linen', 'Casual', 'off_shoulder_top.jpg', 'www.zara.com/offshouldertop', 'Available'),
('Turtleneck Sweater', 'Tops', 'Mango', 50.00, 'Regular', 'Women', 'Minimalist', 'M', 'White', 'Wool', 'Casual', 'turtleneck_sweater.jpg', 'www.mango.com/turtlenecksweater', 'Available'),
('Sequin Blouse', 'Tops', 'H&M', 55.00, 'Regular', 'Women', 'Casual', 'M', 'Yellow', 'Polyester', 'Party', 'sequin_blouse.jpg', 'www.hm.com/sequinblouse', 'Available'),

-- Tops for Unisex
('Henley Shirt', 'Tops', 'Uniqlo', 30.00, 'Regular', 'Unisex', 'Casual', 'M', 'Gray', 'Cotton', 'Casual', 'henley_shirt.jpg', 'www.uniqlo.com/henleyshirt', 'Available'),
('Bomber Jacket', 'Tops', 'Adidas', 100.00, 'Regular', 'Unisex', 'Streetwear', 'L', 'Black', 'Nylon', 'Casual', 'bomber_jacket.jpg', 'www.adidas.com/bomberjacket', 'Available'),
('Zip-Up Hoodie', 'Tops', 'Nike', 70.00, 'Regular', 'Unisex', 'Streetwear', 'M', 'Gray', 'Polyester', 'Casual', 'zipup_hoodie.jpg', 'www.nike.com/zipuphoodie', 'Available'),
('Flannel Shirt', 'Tops', 'Urban Outfitters', 45.00, 'Regular', 'Unisex', 'Streetwear', 'L', 'Red', 'Cotton', 'Casual', 'flannel_shirt.jpg', 'www.urbanoutfitters.com/flannelshirt', 'Available'),
('Tech Fleece Hoodie', 'Tops', 'Nike', 120.00, 'Slim', 'Unisex', 'Sporty', 'L', 'Black', 'Polyester', 'Sport', 'tech_fleece_hoodie.jpg', 'www.nike.com/techfleecehoodie', 'Available'),

-- Bottoms for Men
('Chino Pants', 'Bottoms', 'Gap', 60.00, 'Slim', 'Men', 'Classic', 'M', 'Black', 'Cotton', 'Business Casual', 'chino_pants.jpg', 'www.gap.com/chinopants', 'Available'),
('Cargo Pants', 'Bottoms', 'Uniqlo', 55.00, 'Regular', 'Men', 'Sporty', 'L', 'Green', 'Cotton', 'Casual', 'cargo_pants.jpg', 'www.uniqlo.com/cargopants', 'Available'),
('Tailored Trousers', 'Bottoms', 'Hugo Boss', 150.00, 'Slim', 'Men', 'Business', 'L', 'Black', 'Wool', 'Formal', 'tailored_trousers.jpg', 'www.hugoboss.com/trousers', 'Available'),
('Joggers', 'Bottoms', 'Adidas', 50.00, 'Regular', 'Men', 'Sporty', 'M', 'Gray', 'Polyester', 'Sport', 'joggers.jpg', 'www.adidas.com/joggers', 'Available'),
('Corduroy Pants', 'Bottoms', 'Levis', 80.00, 'Slim', 'Men', 'Vintage', 'L', 'Brown', 'Corduroy', 'Casual', 'corduroy_pants.jpg', 'www.levis.com/corduroypants', 'Available'),

-- Bottoms for Women
('Skinny Jeans', 'Bottoms', 'Zara', 70.00, 'Slim', 'Women', 'Bohemian', 'S', 'Black', 'Denim', 'Casual', 'skinny_jeans.jpg', 'www.zara.com/skinnyjeans', 'Available'),
('Palazzo Pants', 'Bottoms', 'Mango', 80.00, 'Regular', 'Women', 'Bohemian', 'M', 'White', 'Linen', 'Casual', 'palazzo_pants.jpg', 'www.mango.com/palazzopants', 'Available'),
('High-Waisted Shorts', 'Bottoms', 'H&M', 35.00, 'Slim', 'Women', 'Bohemian', 'S', 'Blue', 'Denim', 'Casual', 'highwaisted_shorts.jpg', 'www.hm.com/highwaistedshorts', 'Available'),
('Tapered Trousers', 'Bottoms', 'Uniqlo', 90.00, 'Regular', 'Women', 'Streetwear', 'M', 'Black', 'Cotton', 'Business', 'tapered_trousers.jpg', 'www.uniqlo.com/taperedtrousers', 'Available'),
('Leather Skirt', 'Bottoms', 'H&M', 120.00, 'Slim', 'Women', 'Streetwear', 'M', 'Black', 'Leather', 'Party', 'leather_skirt.jpg', 'www.hm.com/leatherskirt', 'Available'),


('Oxford Shoes', 'Shoes', 'Clarks', 140.00, 'Regular', 'Men', 'Business', '9', 'Black', 'Leather', 'Formal', 'oxford_shoes.jpg', 'www.clarks.com/oxfordshoes', 'Available'),
('Sneakers', 'Shoes', 'Nike', 100.00, 'Regular', 'Men', 'Sporty', '10', 'White', 'Mesh', 'Casual', 'sneakers.jpg', 'www.nike.com/sneakers', 'Available'),
('Boots', 'Shoes', 'Timberland', 180.00, 'Regular', 'Men', 'Business', '11', 'Brown', 'Leather', 'Casual', 'boots.jpg', 'www.timberland.com/boots', 'Available'),
('Loafers', 'Shoes', 'Gucci', 600.00, 'Slim', 'Men', 'Classic', '9', 'Black', 'Leather', 'Formal', 'loafers.jpg', 'www.gucci.com/loafers', 'Available'),
('Running Shoes', 'Shoes', 'Adidas', 120.00, 'Regular', 'Men', 'Sporty', '10', 'Blue', 'Mesh', 'Sport', 'running_shoes.jpg', 'www.adidas.com/runningshoes', 'Available'),

-- Shoes for Women
('Heeled Sandals', 'Shoes', 'Steve Madden', 95.00, 'Slim', 'Women', 'Classic', '7', 'Black', 'Leather', 'Casual', 'heeled_sandals.jpg', 'www.stevemadden.com/heeledsandals', 'Available'),
('Ankle Boots', 'Shoes', 'Zara', 150.00, 'Regular', 'Women', 'Classic', '8', 'Black', 'Leather', 'Casual', 'ankle_boots.jpg', 'www.zara.com/ankleboots', 'Available'),
('Ballet Flats', 'Shoes', 'Tory Burch', 200.00, 'Regular', 'Women', 'Classic', '7', 'Black', 'Leather', 'Formal', 'ballet_flats.jpg', 'www.toryburch.com/balletflats', 'Available'),
('Wedges', 'Shoes', 'Aldo', 85.00, 'Slim', 'Women', 'Trendy', '7', 'Brown', 'Leather', 'Casual', 'wedges.jpg', 'www.aldo.com/wedges', 'Available'),
('Running Shoes', 'Shoes', 'Nike', 110.00, 'Regular', 'Women', 'Sporty', '7', 'Pink', 'Mesh', 'Sport', 'womens_running_shoes.jpg', 'www.nike.com/runningshoes', 'Available'),

-- Shoes for Unisex
('Flip Flops', 'Shoes', 'Havaianas', 30.00, 'Regular', 'Unisex', 'Casual', '8', 'White', 'Rubber', 'Casual', 'flip_flops.jpg', 'www.havaianas.com/flipflops', 'Available'),
('Sneakers', 'Shoes', 'Converse', 60.00, 'Regular', 'Unisex', 'Casual', '9', 'White', 'Canvas', 'Casual', 'converse_sneakers.jpg', 'www.converse.com/sneakers', 'Available'),
('Running Shoes', 'Shoes', 'Asics', 130.00, 'Regular', 'Unisex', 'Sporty', '10', 'Black', 'Mesh', 'Sport', 'asics_running_shoes.jpg', 'www.asics.com/runningshoes', 'Available'),
('Clogs', 'Shoes', 'Crocs', 40.00, 'Regular', 'Unisex', 'Casual', '10', 'Blue', 'Rubber', 'Casual', 'crocs_clogs.jpg', 'www.crocs.com/clogs', 'Available'),
('Slides', 'Shoes', 'Adidas', 45.00, 'Regular', 'Unisex', 'Sporty', '9', 'Black', 'Rubber', 'Casual', 'adidas_slides.jpg', 'www.adidas.com/slides', 'Available'),


-- Accessories for Men
('Leather Belt', 'Accessories', 'Hugo Boss', 75.00, 'Regular', 'Men', 'Classic', 'M', 'Black', 'Leather', 'Formal', 'belt.jpg', 'www.hugoboss.com/leatherbelt', 'Available'),
('Sunglasses', 'Accessories', 'Ray-Ban', 150.00, 'Regular', 'Men', 'Sporty', 'One Size', 'Black', 'Plastic', 'Casual', 'sunglasses.jpg', 'www.ray-ban.com/sunglasses', 'Available'),
('Watch', 'Accessories', 'Fossil', 200.00, 'Regular', 'Men', 'Classic', 'One Size', 'Silver', 'Stainless Steel', 'Formal', 'watch.jpg', 'www.fossil.com/watch', 'Available'),
('Cufflinks', 'Accessories', 'Montblanc', 350.00, 'Regular', 'Men', 'Classic', 'One Size', 'Gray', 'Metal', 'Formal', 'cufflinks.jpg', 'www.montblanc.com/cufflinks', 'Available'),
('Baseball Cap', 'Accessories', 'Nike', 30.00, 'Regular', 'Men', 'Sporty', 'One Size', 'White', 'Cotton', 'Casual', 'cap.jpg', 'www.nike.com/baseballcap', 'Available'),

-- Accessories for Women
('Handbag', 'Accessories', 'Chanel', 3500.00, 'Regular', 'Women', 'Trendy', 'One Size', 'Black', 'Leather', 'Formal', 'handbag.jpg', 'www.chanel.com/handbag', 'Available'),
('Scarf', 'Accessories', 'Hermes', 400.00, 'Slim', 'Women', 'Trendy', 'One Size', 'Red', 'Silk', 'Casual', 'scarf.jpg', 'www.hermes.com/scarf', 'Available'),
('Earrings', 'Accessories', 'Tiffany & Co.', 500.00, 'Regular', 'Women', 'Trendy', 'One Size', 'Gray', 'Sterling Silver', 'Formal', 'earrings.jpg', 'www.tiffany.com/earrings', 'Available'),
('Sunglasses', 'Accessories', 'Gucci', 300.00, 'Regular', 'Women', 'Trendy', 'One Size', 'Black', 'Plastic', 'Casual', 'womens_sunglasses.jpg', 'www.gucci.com/sunglasses', 'Available'),
('Bracelet', 'Accessories', 'Pandora', 120.00, 'Slim', 'Women', 'Trendy', 'One Size', 'White', 'Metal', 'Casual', 'bracelet.jpg', 'www.pandora.com/bracelet', 'Available'),

-- Accessories for Unisex
('Backpack', 'Accessories', 'Herschel', 80.00, 'Regular', 'Unisex', 'Casual', 'One Size', 'Grey', 'Canvas', 'Casual', 'backpack.jpg', 'www.herschel.com/backpack', 'Available'),
('Beanie', 'Accessories', 'Adidas', 25.00, 'Regular', 'Unisex', 'Sporty', 'One Size', 'Black', 'Wool', 'Casual', 'beanie.jpg', 'www.adidas.com/beanie', 'Available'),
('Wallet', 'Accessories', 'Louis Vuitton', 700.00, 'Regular', 'Unisex', 'Classic', 'One Size', 'Brown', 'Leather', 'Formal', 'wallet.jpg', 'www.louisvuitton.com/wallet', 'Available');

/*
INSERT INTO Recommendations (CustomerID, ItemID, RecommendationType)
VALUES
(1, 1, 'Personalized'),
(2, 2, 'Trending');
*/

INSERT INTO FashionInfluencers (FullName, Username, Email, Password, Specialty, ProfilePicture, FacebookLink, Status)
VALUES
('Sophia Lee', 'sophialee', 'sophia@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Chic & Minimalist Fashion', 'sophia.jpg', 'http://instagram.com/sophialee', 'Active'),
('James Kim', 'jameskim', 'james@example.com', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 'Streetwear & Casual Fashion', 'james.jpg', 'http://twitter.com/jameskim', 'Active');

INSERT INTO Courses (CourseName, Description, Price, InstructorID, ThumbnailURL, Status)
VALUES
('Mastering Streetwear', 'A comprehensive course on streetwear fashion.', 200000, 2,'thumbnail.jpg', 'Available'),
('Chic Fashion Basics', 'Learn the basics of chic fashion style.', 400000, 1, 'thumbnail.jpg', 'Available');

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
('Free', null, null, 'Suggest fashion items, Sugges fashion outfits, 24-hour support response time', 'Active'),
('Premium', 49000.00, 60, 'Suggest fashion items, Suggest fashion outfits, Support 24/7, Manage your wardrobe, Save your favorite outfits', 'Active');


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
(2, 1, null, null, 0),
(3, 1, null, null, 1),
(2, 2, '2024-08-15', '2024-11-15', 1), 
(3, 2, '2024-06-01', '2024-08-30', 0);  


