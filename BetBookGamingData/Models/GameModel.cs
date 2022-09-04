

namespace BetBookGamingData.Models;


public class GameModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string GameId { get; set; }
    public int HomeTeamId { get; set; }
    public TeamModel HomeTeam { get; set; }
    public int AwayTeamId { get; set; }
    public TeamModel AwayTeam { get; set; }
    public string Stadium { get; set; }
    public double? PointSpread { get; set; }
    public double? HomeTeamFinalScore { get; set; }
    public double? AwayTeamFinalScore { get; set; }
    public int? GameWinnerId { get; set; }
    public TeamModel GameWinner { get; set; }
    public int WeekNumber { get; set; }
    public SeasonType Season { get; set; }
    public DateTime DateOfGame { get; set; }
    public GameStatus GameStatus { get; set; }
    public int ScoreId { get; set; }
    public string DateOfGameOnly { get; set; }
    public string TimeOfGameOnly { get; set; }

    public string PointSpreadDescription
    {
        get =>
            PointSpread < 0 ?
            $"- {PointSpread?.ToString().Trim('-')} points" :
            $"+ {PointSpread} points";
    }
}


