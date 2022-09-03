

using BetBookGamingData.Dto;

namespace BetBookGamingData.Models;

public class SingleBetForParleyModel
{
    public int Id { get; set; }
    public Guid CreateBetGuid { get; set; }
    public Guid ParleyBetSlipGuid { get; set; }
    public int MoneylinePayout { get; set; }
    public BetType BetType { get; set; }
    public SingleBetForParleyStatus SingleBetForParleyStatus { get; set; }
    public int ScoreIdOfGame { get; set; }
    public GameDto Game { get; set; }
    public string Winner { get; set; }
    public float? PointSpread { get; set; }
    public float? OverUnder { get; set; }
}
