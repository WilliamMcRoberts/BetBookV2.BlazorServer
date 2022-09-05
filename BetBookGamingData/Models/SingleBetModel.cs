

using BetBookGamingData.Dto;

namespace BetBookGamingData.Models;
public class SingleBetModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SingleBetId { get; set; }

    public string BettorId { get; set; }
    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }
    public int MoneylinePayout { get; set; }

    [BsonRepresentation(BsonType.String)]
    public BetType BetType { get; set; }

    [BsonRepresentation(BsonType.String)]
    public SingleBetStatus SingleBetStatus { get; set; }

    [BsonRepresentation(BsonType.String)]
    public SingleBetPayoutStatus SingleBetPayoutStatus { get; set; }

    public GameDto Game { get; set; }
    public int ScoreIdOfGame { get; set; }
    public string WinnerChosen { get; set; }
    public double? PointSpread { get; set; }
    public double? OverUnder { get; set; }
}
