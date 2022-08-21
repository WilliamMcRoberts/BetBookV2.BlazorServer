
namespace BetBookGamingData.Models;

public class OverUnderBetModel : BetModelNew
{
    public float OverUnder { get; set; }
    public double? TotalScoreOfGame { get; set; }
    public OverUnder ChosenWinner { get; set; }
}
