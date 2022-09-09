
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoSingleBetData : IMongoSingleBetData
{
    private readonly IMongoCollection<SingleBetModel> _singleBets;
    private readonly IMongoUserData _userData;
    private readonly ILogger<MongoSingleBetData> _logger;


    public MongoSingleBetData(
        IMongoDbConnection mongoDbConnection, ILogger<MongoSingleBetData> logger, IMongoUserData userData)
    {
        _singleBets = mongoDbConnection.SingleBetsCollection;
        _logger = logger;
        _userData = userData;
    }

    public Task CreateSingleBet(SingleBetModel singleBet, UserModel user)
    {
        _logger.LogInformation("Calling Create Single Bet / MongoSingleBetData");

        user.AccountBalance -= singleBet.BetAmount;

        _userData.UpdateUser(user);
        return _singleBets.InsertOneAsync(singleBet);
    }

    public async Task<List<SingleBetModel>> GetBettorSingleBets(string userId)
    {
        var results = await _singleBets.FindAsync(b => b.BettorId == userId);

        return results.ToList();
    }
}
