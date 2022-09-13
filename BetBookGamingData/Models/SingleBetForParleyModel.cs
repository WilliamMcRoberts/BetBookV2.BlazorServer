

using BetBookGamingData.Dto;

namespace BetBookGamingData.Models;

#nullable enable

public class SingleBetForParleyModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string SingleBetForParleyId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public BetType BetType { get; set; }

    [BsonRepresentation(BsonType.String)]
    public SingleBetForParleyStatus SingleBetForParleyStatus { get; set; }

    public GameSnapshotModel GameSnapshot { get; set; }
    public string BettorId { get; set; }
    public string WinnerChosen { get; set; } = string.Empty;
    public decimal PointsAfterSpread { get; set; }
}

#nullable disable