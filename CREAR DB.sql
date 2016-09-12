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
	endLocation VARCHAR(MAX) NOT NULL
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