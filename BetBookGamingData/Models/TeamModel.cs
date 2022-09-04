
namespace BetBookGamingData.Models;



public class TeamModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string TeamId { get; set; }

    public string TeamName { get; set; }

    public string City { get; set; }

    public string Stadium { get; set; }

    public string Wins { get; set; }

    public string Losses { get; set; }

    public string Draws { get; set; }

    public string Symbol { get; set; }

    public string Division { get; set; }

    public string Conference { get; set; }

    public string ImagePath
    {
        get => Symbol == "BAL" ? $"{Symbol.ToLower()}.png" 
               : $"{Symbol.ToLower()}.svg";
    }

    public string[] TeamWins 
    { 
        get => Wins.Split('|').SkipLast(1).ToArray(); 
    }

    public string[] TeamLosses 
    {
        get => Losses.Split('|').SkipLast(1).ToArray(); 
    }

    public string[] TeamDraws 
    { 
        get => Draws.Split('|').SkipLast(1).ToArray();
    }
}

