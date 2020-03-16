--CREATE TABLE Users(
--Id INT PRIMARY KEY IDENTITY,
--Gamertag NVARCHAR(15) NOT NULL,
--UserName NVARCHAR(30) NOT NULL,
--DOB DATE,
--Gender NVARCHAR(15),
--Images NVARCHAR(250),
--UserID NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id)
--)

--GO

--CREATE TABLE Gamers(
--Id INT PRIMARY KEY IDENTITY,
--Gamertag NVARCHAR(15) NOT NULL,
--UserID NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id),
--TotalKills INT,
--TotalDeaths INT,
--TimeSpentRespawning INT,
--KDRatio FLOAT(53),
--TotalAssists INT,
--KDARatio FLOAT(53),
--TotalAssassinations INT,
--TotalHeadshots INT,
--TotalShotsFired INT,
--TotalShotsLanded INT,
--AccuracyRatio FLOAT(53),
--TotalGamesWon INT,
--TotalGamesLost INT,
--WinLossRatio FLOAT(53),
--TotalGamesTied INT,
--TotalGamesCompleted INT,
--TotalTimePlayed NVARCHAR(25),
--Score INT,
--Ranking INT,
--Images NVARCHAR(250),
--Notes NVARCHAR(500),
--GameTypeIntID INT,
--GameTypeNVarCharID NVARCHAR(50)
--)

--GO

--CREATE TABLE Teams(
--Id INT PRIMARY KEY IDENTITY,
--TeamName NVARCHAR(25),
--Player1 NVARCHAR(15) NOT NULL,
--Player2 NVARCHAR(15) NOT NULL,
--Player3 NVARCHAR(15),
--Player4 NVARCHAR(15),
--Player5 NVARCHAR(15),
--Player6 NVARCHAR(15),
--Player7 NVARCHAR(15),
--Player8 NVARCHAR(15),
--AvgWLRatio FLOAT(53),
--AvgKDRatio FLOAT(53),
--AvgKDARatio FLOAT(53),
--AvgAccRatio FLOAT(53),
--AvgScore FLOAT(53),
--Images NVARCHAR(250),
--Notes NVARCHAR(500),
--UserID NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(Id)
--)

