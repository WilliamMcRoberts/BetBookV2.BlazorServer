
namespace BetBookGamingData.Models;

public class CreateBetModel
{
    public Guid Id { get; set; }
    public BetType BetType { get; set; }
    public decimal BetAmount { get; set; }
    public int MoneylinePayout { get; set; }
    public GameDto Game { get; set; }
    public string Winner { get; set; }
    public decimal? PointSpread { get; set; }
    public decimal? OverUnder { get; set; }
}
