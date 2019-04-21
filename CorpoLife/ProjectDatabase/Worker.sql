CREATE TABLE [dbo].[Worker]
(
	[WorkerID] INT NOT NULL PRIMARY KEY, 
    [Password] VARCHAR(MAX) NOT NULL, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [TeamName] VARCHAR(MAX) NOT NULL, 
    [TeamID] INT NOT NULL, 
    [DepartmentID] INT NOT NULL, 
    [Level] INT NOT NULL, 
    [DepartmentName] VARCHAR(MAX) NOT NULL
)
