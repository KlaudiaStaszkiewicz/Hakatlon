﻿CREATE TABLE [dbo].[Worker]
(
	[WorkerID] INT NOT NULL PRIMARY KEY, 
    [Password] NVARCHAR(20) NOT NULL, 
    [Name] TEXT NOT NULL, 
    [TeamName] TEXT NOT NULL, 
    [TeamID] INT NOT NULL, 
    [Status] INT NOT NULL, 
    [Level] INT NOT NULL
)
