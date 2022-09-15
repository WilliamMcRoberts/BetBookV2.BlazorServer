
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BetBookGamingData.Data;

public class MongoSingleBetData : IMongoSingleBetData
{
    private readonly IMongoCollection<SingleBetModel> _singleBets;
    private readonly IMongoDbConnection _database;
    private readonly IMongoUserData _userData;
    private readonly ILogger<MongoSingleBetData> _logger;


    public MongoSingleBetData(IMongoDbConnection mongoDbConnection,
                              ILogger<MongoSingleBetData> logger,
                              IMongoUserData userData)
    {
        _singleBets = mongoDbConnection.SingleBetsCollection;
        _logger = logger;
        _userData = userData;
        _database = mongoDbConnection;
    }

    public async Task CreateSingleBet(SingleBetModel singleBet)
    {
        _logger.LogInformation("Calling Create Single Bet / MongoSingleBetData");

        var client = _database.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_database.DatabaseName);

            var usersInTransaction = db.GetCollection<UserModel>(
                _database.UsersCollectionName);

            var user = await _userData.GetCurrentUserByUserId(singleBet.BettorId);

            user.AccountBalance -= singleBet.BetAmount;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == singleBet.BettorId, user);

            await _singleBets.InsertOneAsync(singleBet);

            await session.CommitTransactionAsync();

        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Single Bet...Transaction Aborted / SingleBetData");

            await session.AbortTransactionAsync();
        }

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
                b => b.GameSnapshot.ScoreID == scoreIdOfGame && b.SingleBetStatus == SingleBetStatus.IN_PROGRESS);

        return results.ToList();
    }

    public async Task UpdateSingleBet(SingleBetModel singleBet)
    {
        _logger.LogInformation("Calling Update Single Bet / MongoSingleBetData");

        var client = _database.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_database.DatabaseName);

            var usersInTransaction = db.GetCollection<UserModel>(
                _database.UsersCollectionName);

            var user = await _userData.GetCurrentUserByUserId(singleBet.BettorId);

            user.AccountBalance =
                singleBet.SingleBetStatus == SingleBetStatus.WINNER ?
                    user.AccountBalance + singleBet.BetPayout :
                singleBet.SingleBetStatus == SingleBetStatus.PUSH ?
                    user.AccountBalance + singleBet.BetAmount
                    : user.AccountBalance;

            if (singleBet.SingleBetStatus != SingleBetStatus.LOSER)
                singleBet.SingleBetPayoutStatus = SingleBetPayoutStatus.PAID;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == singleBet.BettorId, user);

            var filter = Builders<SingleBetModel>.Filter.Eq(
            "SingleBetId", singleBet.SingleBetId);

            await _singleBets.ReplaceOneAsync(
                filter, singleBet, new ReplaceOptions { IsUpsert = true });

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update Single Bet...Transaction Aborted / SingleBetData");

            await session.AbortTransactionAsync();
        }
    }
}
