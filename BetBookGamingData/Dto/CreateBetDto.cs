

using BetBookGamingData.Dto;

namespace BetBookGamingData.Models;

public class CreateBetDto
{
    public Guid Id { get; set; }
    public BetType BetType { get; set; }
    public decimal BetAmount { get; set; }
    public int MoneylinePayout { get; set; }
    public Game Game { get; set; }
    public string Winner { get; set; }
}
