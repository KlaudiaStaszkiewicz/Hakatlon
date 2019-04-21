CREATE TABLE [dbo].[ScheduleItem]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Team] VARCHAR(MAX) NOT NULL, 
    [TeamID] INT NOT NULL, 
    [Status] VARCHAR(MAX) NOT NULL, 
    [Text] VARCHAR(MAX) NOT NULL 
)
