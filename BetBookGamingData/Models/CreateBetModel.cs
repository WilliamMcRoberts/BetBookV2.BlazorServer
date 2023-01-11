
using System.ComponentModel.DataAnnotations;

namespace BetBookGamingData.Models;

public class CreateBetModel
{
    public Guid Id { get; set; }
    public BetType BetType { get; set; }

    [Required(ErrorMessage = "Bet amount is required")]
    [Range(1, 10000, ErrorMessage = "Bet amount must be at least $1 and less than $10,000")]
    public decimal BetAmount { get; set; }
    public int MoneylinePayout { get; set; }
    public GameDto Game { get; set; }
    public string Winner { get; set; }
    public decimal? PointSpread { get; set; }
    public decimal? OverUnder { get; set; }
}
