

namespace BetBookGamingData.Models;

public class MoneylineBetModel
{
    public int ID { get; set; }
    public int Moneyline { get; set; }
    public decimal BetAmount { get; set; }
    public string ChosenWinner { get; set; }
    public string FinalWinner { get; set; }
    public double? AwayTeamFinalScore { get; set; }
    public double? HomeTeamFinalScore { get; set; }
    public BetStatus BetStatus { get; set; }
    public int ScoreID { get; set; }
    public decimal BetPayout { get; set; }
    public string BetDescription { get; set; }
}
