

namespace BetBookGamingData.Models;

public class BetModelNew
{
    public int ID { get; set; }
    public string AwayTeam { get; set; }
    public string HomeTeam { get; set; }
    public double? AwayTeamFinalScore { get; set; }
    public double? HomeTeamFinalScore { get; set; }
    public BetStatus BetStatus { get; set; }
    public int ScoreID { get; set; }
    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }
    public string BetDescription { get; set; }
    public BetTypeNew BetType { get; set; }
    public int MoneyLinePayout { get; set; }
}
