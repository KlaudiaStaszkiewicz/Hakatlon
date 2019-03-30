CREATE TABLE [dbo].[ScheduleItem]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Team] TEXT NOT NULL, 
    [TeamID] INT NOT NULL, 
    [Status] TEXT NOT NULL, 
    [Text] TEXT NOT NULL 
)
