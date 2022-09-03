

using BetBookGamingData.Dto;

namespace BetBookGamingData.Models;

public class CreateBetModel
{
    public Guid Id { get; set; }
    public BetType BetType { get; set; }
    public decimal BetAmount { get; set; }
    public int MoneylinePayout { get; set; }
    public GameDto Game { get; set; }
    public string Winner { get; set; }
    public float? PointSpread { get; set; }
    public float? OverUnder { get; set; }
}
