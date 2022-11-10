namespace BetBookGamingData.Data;

public interface IMongoSingleBetData
{
    Task<bool> CreateSingleBet(SingleBetModel singleBet);
    Task<List<SingleBetModel>> GetAllSingleBetsOnGameInProgress(int scoreIdOfGame);
    Task<List<SingleBetModel>> GetBettorSingleBets(string userId);
    Task UpdateSingleBet(SingleBetModel singleBet);
}