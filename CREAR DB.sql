CREATE DATABASE ProjectLemon

USE ProjectLemon

CREATE TABLE Users
(
	idUser INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	firstName VARCHAR(20) NOT NULL,
	secondName VARCHAR (20),
	firstLastName VARCHAR (20) NOT NULL,
	secondLastName VARCHAR(20),
	email VARCHAR(50) NOT NULL,
	[password] BINARY(64) NOT NULL,
	cellphoneNumber VARCHAR(20) NOT NULL,
	DOB DATE NOT NULL
)

CREATE TABLE Location
(
	idLocation INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	idUser INT NOT NULL,
	startAddress VARCHAR(MAX) NOT NULL,
	startLocation VARCHAR(MAX) NOT NULL,
	endAddress VARCHAR(MAX) NOT NULL,
	endLocation VARCHAR(MAX) NOT NULL,
	tripTime TIME NOT NULL
)

CREATE TABLE friendRelation 
(
	idUser INT NOT NULL,
	idFriend INT NOT NULL,
	tripsTogether INT
)

CREATE PROCEDURE addUser 
@firstName VARCHAR(20), @secondName VARCHAR(20),  @firstLastName VARCHAR(20), @secondLastName VARCHAR(20), 
@email VARCHAR(50), @password NVARCHAR(64), @cellPhoneNumber VARCHAR(20), @DOB DATE
AS
	BEGIN
		INSERT INTO Users (firstName, secondName, firstLastName, secondLastName, email, [password], cellphoneNumber, DOB)
		VALUES(@firstName, @secondName, @firstLastName, @secondLastName, @email, HASHBYTES('SHA2_512', @password), @cellPhoneNumber, @DOB)
	END
GO

CREATE PROCEDURE addLocation
@idUser INT, @startAddress VARCHAR(MAX), @startLocation VARCHAR(MAX), @endAddress VARCHAR(MAX), @endLocation VARCHAR(MAX), @tripTime TIME
AS
	BEGIN
		INSERT INTO Location (idUser, startAddress, startLocation, endAddress, endLocation, tripTime)
		VALUES (@idUser, @startAddress, @startLocation, @endAddress, @endLocation, @tripTime)
	END
GO

CREATE PROCEDURE addFriendRelation
@idUser INT, @idFriend INT, @tripsTogether INT
AS
	BEGIN
		INSERT INTO friendRelation (idUser, idFriend, tripsTogether)
		VALUES (@idUser, @idFriend, @tripsTogether)
	END
GO