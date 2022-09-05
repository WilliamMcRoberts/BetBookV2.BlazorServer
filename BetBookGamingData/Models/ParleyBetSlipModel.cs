

namespace BetBookGamingData.Models;

public class ParleyBetSlipModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ParleyBetSlipId { get; set; }

    public List<SingleBetForParleyModel> SingleBetsForParleyList { get; set; } = new();
    public decimal ParleyBetAmount { get; set; }
    public decimal ParleyBetPayout { get; set; }

    [BsonRepresentation(BsonType.String)]
    public ParleyBetSlipPayoutStatus ParleyBetSlipPayoutStatus { get; set; }

    [BsonRepresentation(BsonType.String)]
    public ParleyBetSlipStatus ParleyBetSlipStatus { get; set; }
}
