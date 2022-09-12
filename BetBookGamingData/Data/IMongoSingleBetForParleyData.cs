namespace BetBookGamingData.Data
{
    public interface IMongoSingleBetForParleyData
    {
        Task CreateSingleBetForParley(SingleBetForParleyModel singleBetForParley);
        Task<List<SingleBetForParleyModel>> GetAllSingleBetsForParleyOnGameInProgress(int scoreIdOfGame);
        Task UpdateSingleBetForParley(SingleBetForParleyModel singleBetForParley);
    }
}