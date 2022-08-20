using BetBookGamingData.Models;

namespace BetBookGamingData.Interfaces;

#nullable enable

public interface IBetData
{
    Task DeleteBet(int id);
    Task<BetModel?> GetBet(int betId);
    Task<IEnumerable<BetModel>> GetBets();
    Task<int> InsertBet(BetModel bet);
    Task UpdateBet(BetModel bet);
}

#nullable restore
