namespace BetBookGamingData.Data
{
    public interface IMongoSingleBetForParleyData
    {
        Task CreateSingleBetForParley(SingleBetForParleyModel singleBetForParley);
    }
}