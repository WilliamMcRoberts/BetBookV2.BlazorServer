

namespace BetBookGamingData.Models;

public class GameSnapshotModel
{
    public int Week { get; set; }
    public DateTime Date { get; set; }
    public string AwayTeam { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public decimal? AwayScore { get; set; }
    public decimal? HomeScore { get; set; }
    public decimal? PointSpread { get; set; }
    public decimal? OverUnder { get; set; }
    public int? AwayTeamMoneyLine { get; set; }
    public int? HomeTeamMoneyLine { get; set; }
    public int? PointSpreadAwayTeamMoneyLine { get; set; }
    public int? PointSpreadHomeTeamMoneyLine { get; set; }
    public int ScoreID { get; set; }
    public int? OverPayout { get; set; }
    public int? UnderPayout { get; set; }
}
