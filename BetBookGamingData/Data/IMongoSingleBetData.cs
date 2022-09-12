namespace BetBookGamingData.Data
{
    public interface IMongoSingleBetData
    {
        Task CreateSingleBet(SingleBetModel singleBet, UserModel user);
        Task<List<SingleBetModel>> GetAllSingleBetsOnGameInProgress(int scoreIdOfGame);
        Task<List<SingleBetModel>> GetBettorSingleBets(string userId);
        Task UpdateSingleBet(SingleBetModel singleBet);
    }
}