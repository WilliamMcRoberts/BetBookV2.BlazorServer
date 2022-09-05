namespace BetBookGamingData.Data
{
    public interface IMongoSingleBetData
    {
        Task CreateSingleBet(SingleBetModel singleBet);
    }
}