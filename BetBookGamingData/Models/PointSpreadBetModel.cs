

namespace BetBookGamingData.Models;

public class PointSpreadBetModel
{
    public int ID { get; set; }
    public float PointSpread { get; set; }
    public int MoneyLinePayout { get; set; }
    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }
    public int ScoreID { get; set; }
    public double? AwayTeamFinalScore { get; set; }
    public double? HomeTeamFinalScore { get; set; }
    public BetStatus BetStatus { get; set; }
    public string ChosenWinner { get; set; }
    public string BetDescription { get; set; }
}
