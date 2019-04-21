CREATE TABLE [dbo].[Team]
(
	[TeamID] INT NOT NULL PRIMARY KEY, 
    [TeamName] VARCHAR(MAX) NOT NULL, 
    [DepartmentID] INT NOT NULL, 
    [DepartmentName] VARCHAR(MAX) NOT NULL
)
