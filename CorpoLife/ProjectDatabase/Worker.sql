CREATE TABLE [dbo].[Worker]
(
	[WorkerID] INT NOT NULL PRIMARY KEY, 
    [Password] TEXT NOT NULL, 
    [Name] TEXT NOT NULL, 
    [TeamName] TEXT NOT NULL, 
    [TeamID] INT NOT NULL, 
    [DepartmentID] INT NOT NULL, 
    [Level] INT NOT NULL, 
    [DepartmentName] TEXT NOT NULL
)
