

using BetBookGamingData.Dto;

namespace BetBookGamingData.Models;

#nullable enable

public class SingleBetModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SingleBetId { get; set; }

    public string BettorId { get; set; }
    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }
    public decimal PointsAfterSpread { get; set; }
    public GameSnapshotModel GameSnapshot { get; set; }


    [BsonRepresentation(BsonType.String)]
    public BetType BetType { get; set; }

    [BsonRepresentation(BsonType.String)]
    public SingleBetStatus SingleBetStatus { get; set; }

    [BsonRepresentation(BsonType.String)]
    public SingleBetPayoutStatus SingleBetPayoutStatus { get; set; }

    public string WinnerChosen { get; set; } = string.Empty;
}

#nullable disable