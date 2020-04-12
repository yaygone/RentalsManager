----------------------------
------RELATIONAL MODEL------
----------------------------

--Rental(id, startDate, returnDate, condition, customerID, managerUsername, boardgameID)
--Customer(username, fname, lname, password, phone, email, streetAddress, city, bankAcct, goodness, joinDate, dob, gender, maxRentNum)
--Genre(name)
--Boardgame(id, name, price, rating, minPlayers, maxPlayers, playTimeMins, releaseYear, avail)
--Publisher(name)
--Designer(name)
--Manager(username, password)
--GenrePref(customerUsername, genreName)
--GameGenreValue(boardgameID, genreName)
--Review(customerUsername, boardgameID, date, content, rating)
--GamePublishedBy(boardgameID, publisherName)
--GameDesignedBy(boardgameID, designerName)



------------------------------------
------DATABASE AND TABLE SETUP------
------------------------------------

--DROP DATABASE BoardgameRentalRecords
CREATE DATABASE BoardgameRentalRecords

--DROP TABLE GamePublishedBy
--DROP TABLE GameDesignedBy
--DROP TABLE Rental
--DROP TABLE Review
--DROP TABLE GenrePref
--DROP TABLE GameGenreValue
--DROP TABLE Customer
--DROP TABLE Genre
--DROP TABLE Boardgame
--DROP TABLE Manager
--DROP TABLE Publisher
--DROP TABLE Designer

CREATE TABLE Designer
(
	designerName nvarchar(32) NOT NULL

	PRIMARY KEY (designerName)
)

CREATE TABLE Publisher
(
	publisherName nvarchar(32) NOT NULL

	PRIMARY KEY (publisherName)
)

CREATE TABLE Manager
(
	username nvarchar(32) NOT NULL,
	password nvarchar(32) NOT NULL

	PRIMARY KEY (username)
)

CREATE TABLE Boardgame
(
	id int NOT NULL,
	name nvarchar(32) NOT NULL,
	price money,
	rating float,
	minPlayers int,
	maxPlayers int,
	playTimeMins int,
	releaseYear char(4),
	avail bit

	PRIMARY KEY (id),
	CHECK (rating >= 0 AND rating <= 10),
	CHECK (ISNUMERIC(releaseYear) = 1)
)

CREATE TABLE Genre
(
	name nvarchar(32) NOT NULL

	PRIMARY KEY (name)
)

CREATE TABLE Customer
(
	username nvarchar(32) NOT NULL,
	fname nvarchar(32) NOT NULL,
	lname nvarchar(32),
	password nvarchar(32) NOT NULL,
	phone nvarchar(13) NOT NULL,
	email nvarchar(32),
	streetAddress nvarchar(32),
	city nvarchar(32),
	bankAcct nvarchar(19) NOT NULL,
	goodness int,
	joinDate date NOT NULL,
	dob date,
	gender nvarchar(10),
	maxRentNum int

	PRIMARY KEY (username),
	CHECK (ISNUMERIC (phone) = 1),
	CHECK (email LIKE '%_@%_.%_'),
	CHECK (bankAcct LIKE '[0-9][0-9]-[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]'),
	CHECK (goodness >= 0 AND goodness <= 10),
	CHECK (gender IN ('male', 'female', 'non-binary'))
)

CREATE TABLE GameGenreValue
(
	boardgameID int NOT NULL,
	genreName nvarchar(32) NOT NULL

	PRIMARY KEY (boardgameID, genreName),
	FOREIGN KEY (boardgameID) REFERENCES Boardgame,
	FOREIGN KEY (genreName) REFERENCES Genre
)

CREATE TABLE GenrePref
(
	customerUsername nvarchar(32) NOT NULL,
	genreName nvarchar(32) NOT NULL

	PRIMARY KEY (customerUsername, genreName),
	FOREIGN KEY (customerUsername) REFERENCES Customer,
	FOREIGN KEY (genreName) REFERENCES Genre
)

CREATE TABLE Review
(
	customerUsername nvarchar(32) NOT NULL,
	boardgameID int NOT NULL,
	dateReviewed date,
	content nvarchar(512),
	rating int

	PRIMARY KEY (customerUsername, boardgameID),
	CHECK (rating >= 0 AND rating <= 10),
	FOREIGN KEY (customerUsername) REFERENCES Customer,
	FOREIGN KEY (boardgameID) REFERENCES Boardgame
)

CREATE TABLE Rental
(
	id int IDENTITY(1, 1) NOT NULL,
	startDate date NOT NULL,
	returnDate date,
	condition nvarchar(32),
	customerUsername nvarchar(32) NOT NULL,
	managerUsername nvarchar(32) NOT NULL,
	boardgameID int NOT NULL
	
	PRIMARY KEY (id),
	FOREIGN KEY (customerUsername) REFERENCES Customer,
	FOREIGN KEY (managerUsername) REFERENCES Manager,
	FOREIGN KEY (boardgameID) REFERENCES Boardgame
)

CREATE TABLE GamePublishedBy
(
	boardgameID int NOT NULL,
	publisherName nvarchar(32) NOT NULL

	PRIMARY KEY (boardgameID, publisherName),
	FOREIGN KEY (boardgameID) REFERENCES Boardgame,
	FOREIGN KEY (publisherName) REFERENCES Publisher
)

CREATE TABLE GameDesignedBy
(
	boardgameID int NOT NULL,
	designerName nvarchar(32) NOT NULL

	PRIMARY KEY (boardgameID, designerName),
	FOREIGN KEY (boardgameID) REFERENCES Boardgame,
	FOREIGN KEY (designerName) REFERENCES Designer
)



----------------------------------
------INSERTIONS AND OUTPUTS------
----------------------------------

INSERT INTO Publisher VALUES
('999 Games'),
('Ace Studios'),
('ADC Blackfire Entertainment'),
('Adventureland Games'),
('Angry Lion Games'),
('Arclight'),
('Arrakis Games'),
('Asmodee'),
('AURUM, Inc.'),
('Bard Centrum Gier'),
('Bézier Games'),
('BoardM Factory'),
('Brain Games'),
('Broadway Toys LTD'),
('Capstone Games'),
('Cocktail Games'),
('Compaya.hu - Gamer Café Kft.'),
('Conclave Editora'),
('Cranio Creations'),
('CrowD Games'),
('Delta Vision Publishing'),
('Devir'),
('DiceTree Games'),
('Divercentro'),
('dlp games'),
('Edge Entertainment'),
('Ediciones MasQueOca'),
('eggertspiele'),
('Fantasmagoria'),
('Feuerland Spiele'),
('Filosofia Éditions'),
('Fishbone Games'),
('FoxMind Israel'),
('FryxGames'),
('FunBox Jogos'),
('Funforge'),
('Galápagos Jogos'),
('Game Harbor'),
('Gameland 游戏大陆'),
('Games Factory'),
('Game''s Up'),
('Gamewright'),
('Gém Klub Kft.'),
('Gemenot'),
('Ghenos Games'),
('Gigamic'),
('Greater Than Games'),
('Hemz Universal Games Co. Ltd.'),
('Hobby Japan'),
('Hobby World'),
('HomoLudicus'),
('Intrafin Games'),
('KADABRA'),
('Kaissa Chess & Games'),
('Kanga Games'),
('Kilogames'),
('Korea Boardgames co., Ltd.'),
('Lacerta'),
('Lautapelit.fi'),
('Lavka Games'),
('Lex Games'),
('Lifestyle Boardgames Ltd'),
('Lookout Games'),
('Ludicus'),
('Ludofy Creative'),
('Maldito Games'),
('Mandala Jogos'),
('Matagot'),
('Mayfair Games'),
('Meeple BR Jogos'),
('MINDOK'),
('MYBG Co., Ltd.'),
('NeoTroy Games'),
('Next Move Games'),
('Orangutan Games'),
('Pegasus Spiele'),
('Plan B Games'),
('Playfun Games'),
('Ravensburger Spieleverlag GmbH'),
('Rebel'),
('Reflexshop'),
('Regatul Jocurilor'),
('Schwerkraft-Verlag'),
('Siam Board Games'),
('Smart Ltd'),
('Stonemaier Games'),
('Stratelibri'),
('Stronghold Games'),
('Super Meeple'),
('Surfin'' Meeple'),
('Surfin'' Meeple China'),
('Swan Panasia Co., Ltd.'),
('Tower Tactic Games'),
('TWOPLUS Games'),
('uplay.it edizioni'),
('Viravi Edicions'),
('White Goblin Games'),
('YOKA Games'),
('Ystari Games'),
('Z-Man Games, Inc.'),
('Zoch Verlag'),
('Zvezda'),
('テンデイズゲームズ (Ten Days Games)')

INSERT INTO Designer VALUES
('Uwe Rosenberg'),
('Philippe Guérin'), 
('Chris Quilliams'),
('Jens Drögemüller'), 
('Helge Ostertag'),
('Alexander Pfister'),
('Ted Alspach'), 
('Akihisa Okui'),
('R. Eric Reuss'),
('Phil Walker-Harding'),
('Jacob Fryxelius'),
('Elizabeth Hargrave')

INSERT INTO Genre VALUES
('Abstract Strategy'),
('Age of Reason'),
('American West'),
('Animals'),
('Bluffing'),
('Card Drafting'),
('Card Game'),
('Civilization'),
('Deduction'),
('Drafting'),
('Economic'),
('Educational'),
('Environmental'),
('Exploration'),
('Fantasy'),
('Farming'),
('Fighting'),
('Hand Management'),
('Horror'),
('Industry / Manufacturing'),
('Mythology'),
('Party Game'),
('Renaissance'),
('Science Fiction'),
('Set Collection'),
('Simultaneous Action Selection'),
('Space Exploration'),
('Territory Building')

INSERT INTO Boardgame VALUES
(31260, 'Agricola', NULL, 7.96682, 1, 5, 150, 2007, NULL),
(230802, 'Azul', NULL, 7.85835, 2, 4, 45, 2017, NULL),
(102794, 'Caverna: The Cave Farmers', NULL, 8.0392, 1, 7, 210, 2013, NULL),
(220308, 'Gaia Project', NULL, 8.49592, 1, 4, 150, 2017, NULL),
(193738, 'Great Western Trail', NULL, 8.28423, 2, 4, 150, 2016, NULL),
(276025, 'Maracaibo', NULL, 8.27257, 1, 4, 120, 2019, NULL),
(204431, 'One Night Ultimate Alien', NULL, 7.07577, 4, 10, 10, 2017, NULL),
(180956, 'One Night Ultimate Vampire', NULL, 6.74924, 3, 10, 10, 2015, NULL),
(147949, 'One Night Ultimate Werewolf', NULL, 7.16687, 3, 10, 10, 2014, NULL),
(162886, 'Spirit Island', NULL, 8.31674, 1, 4, 120, 2017, NULL),
(192291, 'Sushi Go Party!', NULL, 7.48268, 2, 8, 20, 2016, NULL),
(133473, 'Sushi Go!', NULL, 7.05894, 2, 5, 15, 2013, NULL),
(120677, 'Terra Mystica', NULL, 8.16544, 2, 5, 150, 2012, NULL),
(167791, 'Terraforming Mars', NULL, 8.42498, 1, 5, 120, 2016, NULL),
(266192, 'Wingspan', NULL, 8.1218, 1, 5, 70, 2019, NULL)

INSERT INTO GamePublishedBy VALUES
(31260, 'Lookout Games'),
(31260, '999 Games'),
(31260, 'Brain Games'),
(31260, 'Compaya.hu - Gamer Café Kft.'),
(31260, 'Devir'),
(31260, 'Filosofia Éditions'),
(31260, 'Funforge'),
(31260, 'Hobby Japan'),
(31260, 'Hobby World'),
(31260, 'HomoLudicus'),
(31260, 'Korea Boardgames co., Ltd.'),
(31260, 'Lacerta'),
(31260, 'MINDOK'),
(31260, 'Smart Ltd'),
(31260, 'Stratelibri'),
(31260, 'Swan Panasia Co., Ltd.'),
(31260, 'Ystari Games'),
(31260, 'Z-Man Games, Inc.'),
(230802, 'Next Move Games'),
(230802, 'Plan B Games'),
(230802, 'Asmodee'),
(230802, 'Broadway Toys LTD'),
(230802, 'Divercentro'),
(230802, 'Galápagos Jogos'),
(230802, 'Gém Klub Kft.'),
(230802, 'Ghenos Games'),
(230802, 'Hobby Japan'),
(230802, 'KADABRA'),
(230802, 'Kaissa Chess & Games'),
(230802, 'Korea Boardgames co., Ltd.'),
(230802, 'Lacerta'),
(230802, 'MINDOK'),
(230802, 'Orangutan Games'),
(230802, 'Pegasus Spiele'),
(230802, 'Tower Tactic Games'),
(230802, 'TWOPLUS Games'),
(230802, 'Zvezda'),
(102794, 'Lookout Games'),
(102794, '999 Games'),
(102794, 'CrowD Games'),
(102794, 'Devir'),
(102794, 'Filosofia Éditions'),
(102794, 'Funforge'),
(102794, 'Gemenot'),
(102794, 'Hobby Japan'),
(102794, 'HomoLudicus'),
(102794, 'Korea Boardgames co., Ltd.'),
(102794, 'Lacerta'),
(102794, 'Ludofy Creative'),
(102794, 'Mayfair Games'),
(102794, 'MINDOK'),
(102794, 'Swan Panasia Co., Ltd.'),
(102794, 'uplay.it edizioni'),
(220308, 'Feuerland Spiele'),
(220308, 'Cranio Creations'),
(220308, 'DiceTree Games'),
(220308, 'Edge Entertainment'),
(220308, 'Game Harbor'),
(220308, 'Games Factory'),
(220308, 'Hobby World'),
(220308, 'Maldito Games'),
(220308, 'Mandala Jogos'),
(220308, 'Reflexshop'),
(220308, 'テンデイズゲームズ (Ten Days Games)'),
(220308, 'White Goblin Games'),
(220308, 'Z-Man Games, Inc.'),
(193738, 'eggertspiele'),
(193738, '999 Games'),
(193738, 'Arclight'),
(193738, 'Broadway Toys LTD'),
(193738, 'Conclave Editora'),
(193738, 'Delta Vision Publishing'),
(193738, 'Ediciones MasQueOca'),
(193738, 'Gigamic'),
(193738, 'Korea Boardgames co., Ltd.'),
(193738, 'Lacerta'),
(193738, 'Ludicus'),
(193738, 'MINDOK'),
(193738, 'Pegasus Spiele'),
(193738, 'Plan B Games'),
(193738, 'Stronghold Games'),
(193738, 'uplay.it edizioni'),
(193738, 'Zvezda'),
(276025, 'Game''s Up'),
(276025, 'BoardM Factory'),
(276025, 'Capstone Games'),
(276025, 'dlp games'),
(276025, 'Ediciones MasQueOca'),
(276025, 'Fishbone Games'),
(276025, 'Super Meeple'),
(276025, 'YOKA Games'),
(204431, 'Bézier Games'),
(180956, 'Bézier Games'),
(180956, 'Ravensburger Spieleverlag GmbH'),
(180956, 'White Goblin Games'),
(147949, 'Bézier Games'),
(147949, 'Lacerta'),
(147949, 'Playfun Games'),
(147949, 'Ravensburger Spieleverlag GmbH'),
(147949, 'Reflexshop'),
(147949, 'Siam Board Games'),
(147949, 'Viravi Edicions'),
(147949, 'White Goblin Games'),
(162886, 'Greater Than Games'),
(162886, 'Ace Studios'),
(162886, 'Arrakis Games'),
(162886, 'BoardM Factory'),
(162886, 'Gém Klub Kft.'),
(162886, 'Ghenos Games'),
(162886, 'Hobby World'),
(162886, 'Intrafin Games'),
(162886, 'Lacerta'),
(162886, 'Pegasus Spiele'),
(192291, 'Gamewright'),
(192291, 'Devir'),
(192291, 'Rebel'),
(192291, 'Reflexshop'),
(192291, 'uplay.it edizioni'),
(192291, 'White Goblin Games'),
(192291, 'Zoch Verlag'),
(133473, 'Adventureland Games'),
(133473, 'ADC Blackfire Entertainment'),
(133473, 'AURUM, Inc.'),
(133473, 'Cocktail Games'),
(133473, 'Devir'),
(133473, 'FoxMind Israel'),
(133473, 'Gameland 游戏大陆'),
(133473, 'Gamewright'),
(133473, 'Hemz Universal Games Co. Ltd.'),
(133473, 'Kanga Games'),
(133473, 'Lifestyle Boardgames Ltd'),
(133473, 'NeoTroy Games'),
(133473, 'Rebel'),
(133473, 'Reflexshop'),
(133473, 'uplay.it edizioni'),
(133473, 'White Goblin Games'),
(133473, 'Zoch Verlag'),
(120677, 'Feuerland Spiele'),
(120677, 'Bard Centrum Gier'),
(120677, 'Cranio Creations'),
(120677, 'Devir'),
(120677, 'Filosofia Éditions'),
(120677, 'FunBox Jogos'),
(120677, 'Gém Klub Kft.'),
(120677, 'HomoLudicus'),
(120677, 'Mandala Jogos'),
(120677, 'MINDOK'),
(120677, 'Swan Panasia Co., Ltd.'),
(120677, 'テンデイズゲームズ (Ten Days Games)'),
(120677, 'White Goblin Games'),
(120677, 'Z-Man Games, Inc.'),
(120677, 'Zvezda'),
(167791, 'FryxGames'),
(167791, 'Arclight'),
(167791, 'Fantasmagoria'),
(167791, 'Ghenos Games'),
(167791, 'Intrafin Games'),
(167791, 'Kilogames'),
(167791, 'Korea Boardgames co., Ltd.'),
(167791, 'Lautapelit.fi'),
(167791, 'Lavka Games'),
(167791, 'Lex Games'),
(167791, 'Maldito Games'),
(167791, 'Meeple BR Jogos'),
(167791, 'MINDOK'),
(167791, 'MYBG Co., Ltd.'),
(167791, 'NeoTroy Games'),
(167791, 'Rebel'),
(167791, 'Reflexshop'),
(167791, 'Schwerkraft-Verlag'),
(167791, 'Siam Board Games'),
(167791, 'Stronghold Games'),
(266192, 'Stonemaier Games'),
(266192, '999 Games'),
(266192, 'Angry Lion Games'),
(266192, 'Delta Vision Publishing'),
(266192, 'Divercentro'),
(266192, 'Feuerland Spiele'),
(266192, 'Ghenos Games'),
(266192, 'Lautapelit.fi'),
(266192, 'Lavka Games'),
(266192, 'Ludofy Creative'),
(266192, 'Maldito Games'),
(266192, 'Matagot'),
(266192, 'MINDOK'),
(266192, 'Rebel'),
(266192, 'Regatul Jocurilor'),
(266192, 'Siam Board Games'),
(266192, 'Surfin'' Meeple'),
(266192, 'Surfin'' Meeple China')

INSERT INTO GameDesignedBy VALUES
(31260, 'Uwe Rosenberg'),
(230802, 'Philippe Guérin'), 
(230802, 'Chris Quilliams'),
(102794, 'Uwe Rosenberg'),
(220308, 'Jens Drögemüller'), 
(220308, 'Helge Ostertag'),
(193738, 'Alexander Pfister'),
(276025, 'Alexander Pfister'),
(204431, 'Ted Alspach'), 
(204431, 'Akihisa Okui'),
(180956, 'Ted Alspach'), 
(180956, 'Akihisa Okui'),
(147949, 'Ted Alspach'), 
(147949, 'Akihisa Okui'),
(162886, 'R. Eric Reuss'),
(192291, 'Phil Walker-Harding'),
(133473, 'Phil Walker-Harding'),
(120677, 'Jens Drögemüller'), 
(120677, 'Helge Ostertag'),
(167791, 'Jacob Fryxelius'),
(266192, 'Elizabeth Hargrave')

INSERT INTO GameGenreValue VALUES
(31260, 'Animals'),
(31260, 'Economic'),
(31260, 'Farming'),
(230802, 'Abstract Strategy'),
(230802, 'Renaissance'),
(102794, 'Animals'),
(102794, 'Economic'),
(102794, 'Fantasy'),
(102794, 'Farming'),
(220308, 'Civilization'),
(220308, 'Economic'),
(220308, 'Science Fiction'),
(220308, 'Space Exploration'),
(220308, 'Territory Building'),
(193738, 'American West'),
(193738, 'Animals'),
(276025, 'Economic'),
(276025, 'Exploration'),
(204431, 'Bluffing'),
(204431, 'Card Game'),
(204431, 'Deduction'),
(204431, 'Horror'),
(204431, 'Party Game'),
(180956, 'Bluffing'),
(180956, 'Card Game'),
(180956, 'Deduction'),
(180956, 'Horror'),
(180956, 'Party Game'),
(147949, 'Bluffing'),
(147949, 'Card Game'),
(147949, 'Deduction'),
(147949, 'Horror'),
(147949, 'Party Game'),
(162886, 'Age of Reason'),
(162886, 'Environmental'),
(162886, 'Fantasy'),
(162886, 'Fighting'),
(162886, 'Mythology'),
(162886, 'Territory Building'),
(192291, 'Card Drafting'),
(192291, 'Hand Management'),
(192291, 'Set Collection'),
(192291, 'Simultaneous Action Selection'),
(133473, 'Card Drafting'),
(133473, 'Drafting'),
(133473, 'Hand Management'),
(133473, 'Set Collection'),
(133473, 'Simultaneous Action Selection'),
(120677, 'Civilization'),
(120677, 'Economic'),
(120677, 'Fantasy'),
(120677, 'Territory Building'),
(167791, 'Economic'),
(167791, 'Environmental'),
(167791, 'Industry / Manufacturing'),
(167791, 'Science Fiction'),
(167791, 'Space Exploration'),
(167791, 'Territory Building'),
(266192, 'Animals'),
(266192, 'Card Game'),
(266192, 'Economic'),
(266192, 'Educational')

INSERT INTO Manager VALUES
('mgr1', 'pa55w0rd'),
('mgr2', '1234567'),
('mgr3', 'qwerty')

INSERT INTO Customer VALUES
('user1', 'Alex', 'Anderson', 'password1', '0210210211', 'alex@email.com', '1 Street', 'Hamilton', '00-0000-0000000-001', NULL, '2020-01-01', '2000-01-01', 'male', 2),
('user2', 'Bob', 'Bowers', 'password2', '0210210212', 'bob@email.com', '2 Street', 'Hamilton', '00-0000-0000000-002', NULL, '2020-01-02', '2000-02-02', 'male', 2),
('user3', 'Cat', 'Carlton', 'password3', '0210210213', 'cat@email.com', '3 Street', 'Hamilton', '00-0000-0000000-003', NULL, '2020-01-03', '2000-03-03', 'female', 2),
('user4', 'Denny', 'Davies', 'password4', '0210210214', 'denny@email.com', '4 Street', 'Hamilton', '00-0000-0000000-004', NULL, '2020-01-04', '2000-04-04', 'non-binary', 2),
('user5', 'Ellie', 'Eastman', 'password5', '0210210215', 'ellie@email.com', '5 Street', 'Hamilton', '00-0000-0000000-005', NULL, '2020-01-05', '2000-05-05', 'female', 2),
('user6', 'Fred', 'Farnham', 'password6', '0210210216', 'fred@email.com', '6 Street', 'Hamilton', '00-0000-0000000-006', NULL, '2020-01-06', '2000-06-06', 'male', 2),
('user7', 'Graham', 'Graves', 'password7', '0210210217', 'graham@email.com', '7 Street', 'Hamilton', '00-0000-0000000-007', NULL, '2020-01-07', '2000-07-07', 'male', 2),
('user8', 'Harriet', 'Howard', 'password8', '0210210218', 'harriet@email.com', '8 Street', 'Hamilton', '00-0000-0000000-008', NULL, '2020-01-08', '2000-08-08', 'female', 2),
('user9', 'Ingrid', 'Ivarson', 'password9', '0210210219', 'ingrid@email.com', '9 Street', 'Hamilton', '00-0000-0000000-009', NULL, '2020-01-09', '2000-09-09', 'non-binary', 2),
('user0', 'John', 'Jameson', 'password0', '0210210220', 'john@email.com', '10 Street', 'Hamilton', '00-0000-0000000-010', NULL, '2020-01-10', '2000-10-10', 'male', 2)

INSERT INTO GenrePref VALUES
('user1', 'Animals'),
('user1', 'Environmental'),
('user1', 'Farming'),
('user1', 'Mythology'),
('user3', 'Bluffing'),
('user3', 'Deduction'),
('user3', 'Party Game'),
('user7', 'Science Fiction'),
('user7', 'Space Exploration')

INSERT INTO Review VALUES
('user2', 230802, '2020-03-03', 'Great game, would recommend for playing at parties', 8),
('user7', 266192, '2020-03-05', 'I like birds', 10),
('user9', 147949, '2020-03-08', 'I am never speaking to my friends ever again', 6)

INSERT INTO Rental VALUES
('2020-02-20', '2020-03-01', NULL, 'user2', 'mgr2', 230802),
('2020-02-22', '2020-03-04', NULL, 'user7', 'mgr3', 266192),
('2020-02-21', '2020-03-02', 'minor damage to packaging only', 'user9', 'mgr1', 147949),
('2020-01-31', '2020-02-14', 'missing red board piece', 'user5', 'mgr2', 167791),
('2020-01-11', '2020-01-23', NULL, 'user2', 'mgr1', 220308)

SELECT * FROM Publisher
SELECT * FROM Designer
SELECT * FROM Genre
SELECT * FROM Boardgame
SELECT * FROM GamePublishedBy
SELECT * FROM GameDesignedBy
SELECT * FROM GameGenreValue
SELECT * FROM Manager
SELECT * FROM Customer
SELECT * FROM GenrePref
SELECT * FROM Review
SELECT * FROM Rental