
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

    public async Task CreateSingleBet(SingleBetModel singleBet, UserModel user)
    {
        _logger.LogInformation("Calling Create Single Bet / MongoSingleBetData");

        user.AccountBalance -= singleBet.BetAmount;

        await _userData.UpdateUser(user);
        await _singleBets.InsertOneAsync(singleBet);
    }

    public async Task<List<SingleBetModel>> GetBettorSingleBets(string userId)
    {
        var results = await _singleBets.FindAsync(b => b.BettorId == userId);

        return results.ToList();
    }

    public async Task<List<SingleBetModel>> GetAllSingleBetsOnGameInProgress(int scoreIdOfGame)
    {
        var results =
            await _singleBets.FindAsync(
                b => b.ScoreIdOfGame == scoreIdOfGame && b.SingleBetStatus == SingleBetStatus.IN_PROGRESS);

        return results.ToList();
    }

    public async Task UpdateSingleBet(SingleBetModel singleBet)
    {
        _logger.LogInformation("Calling Update Single Bet / MongoSingleBetData");

        UserModel user =
                 await _userData.GetCurrentUserByUserId(singleBet.BettorId);

        user.AccountBalance =
                singleBet.SingleBetStatus == SingleBetStatus.WINNER ?
                    user.AccountBalance + singleBet.BetPayout :
                singleBet.SingleBetStatus == SingleBetStatus.PUSH ?
                    user.AccountBalance + singleBet.BetAmount
                    : user.AccountBalance;


        if (singleBet.SingleBetStatus != SingleBetStatus.LOSER)
        {
            await _userData.UpdateUser(user);
            singleBet.SingleBetPayoutStatus = SingleBetPayoutStatus.PAID;
        }

        var filter = Builders<SingleBetModel>.Filter.Eq(
            "SingleBetId", singleBet.SingleBetId);

        await _singleBets.ReplaceOneAsync(
            filter, singleBet, new ReplaceOptions { IsUpsert = true });
    }
}
