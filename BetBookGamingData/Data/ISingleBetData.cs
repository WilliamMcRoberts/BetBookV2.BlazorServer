using BetBookGamingData.Models;

namespace BetBookGamingData.Data
{
    public interface ISingleBetData
    {
        Task<SingleBetModel> GetSingleBetById(int singleBetId);
        Task<IEnumerable<SingleBetModel>> GetSingleBets();
        Task<int> InsertSingleBet(SingleBetModel singleBet);
        Task UpdateSingleBet(SingleBetModel singleBet);
    }
}