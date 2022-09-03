CREATE TABLE [dbo].[SingleBets]
(
	[SingleBetId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CreateBetGuid] NVARCHAR(50) NOT NULL, 
    [BetAmount] MONEY NOT NULL, 
    [BetPayout] MONEY NOT NULL, 
    [MoneylinePayout] INT NOT NULL, 
    [BetType] NVARCHAR(50) NOT NULL, 
    [SingleBetStatus] NVARCHAR(50) NOT NULL, 
    [ScoreIdOfGame] INT NOT NULL, 
    [WinnerChosen] NVARCHAR(50) NOT NULL, 
    [PointSpread] FLOAT NULL, 
    [OverUnder] FLOAT NULL, 
    [SingleBetPayoutStatus] NVARCHAR(50) NOT NULL
)
