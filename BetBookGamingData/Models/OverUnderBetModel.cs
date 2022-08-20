
namespace BetBookGamingData.Models;

public class OverUnderBetModel
{
    public int ID { get; set; }
    public float OverUnder { get; set; }
    public int MoneyLinePayout { get; set; }
    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }
    public int ScoreID { get; set; }
    public double? TotalScoreOfGame { get; set; }
    public double? AwayTeamFinalScore { get; set; }
    public double? HomeTeamFinalScore { get; set; }
    public BetStatus BetStatus { get; set; }
    public OverUnder ChosenWinner { get; set; }
    public string BetDescription { get; set; }
}
