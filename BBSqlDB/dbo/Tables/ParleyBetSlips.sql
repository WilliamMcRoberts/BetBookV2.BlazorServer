CREATE TABLE [dbo].[ParleyBetSlips]
(
	[ParleyBetSlipId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SingleBetForParley1Id] INT NOT NULL, 
    [SingleBetForParley2Id] INT NOT NULL, 
    [SingleBetForParley3Id] INT NULL, 
    [SingleBetForParley4Id] INT NULL, 
    [SingleBetForParley5Id] INT NULL, 
    [SingleBetForParley6Id] INT NULL, 
    [SingleBetForParley7Id] INT NULL, 
    [SingleBetForParley8Id] INT NULL, 
    [SingleBetForParley9Id] INT NULL, 
    [SingleBetForParley10Id] INT NULL, 
    [SingleBetForParley11Id] INT NULL, 
    [SingleBetForParley12Id] INT NULL, 
    [ParleyBetAmount] MONEY NOT NULL, 
    [ParleyBetPayout] MONEY NOT NULL, 
    [ParleyBetSlipPayoutStatus] NVARCHAR(50) NOT NULL, 
    [ParleyBetSlipStatus] NVARCHAR(50) NOT NULL
)
