namespace BetBookGamingData.Data
{
    public interface IMongoSingleBetData
    {
        Task CreateSingleBet(SingleBetModel singleBet, UserModel user);
        Task<List<SingleBetModel>> GetBettorSingleBets(string userId);
    }
}