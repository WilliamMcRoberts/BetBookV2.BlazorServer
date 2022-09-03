CREATE TABLE [dbo].[SingleBetsForParley]
(
	[SingleBetForParleyId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CreateBetGuid] NVARCHAR(50) NOT NULL, 
    [ParleyBetSlipGuid] NVARCHAR(50) NOT NULL, 
    [MoneylinePayout] INT NOT NULL, 
    [BetType] NVARCHAR(50) NOT NULL, 
    [SingleBetForParleyStatus] NVARCHAR(50) NOT NULL, 
    [ScoreIdOfGame] INT NOT NULL, 
    [WinnerChosen] NVARCHAR(50) NOT NULL, 
    [PointSpread] FLOAT NULL, 
    [OverUnder] FLOAT NULL
)
