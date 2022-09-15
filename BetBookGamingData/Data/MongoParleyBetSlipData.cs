
using BetBookGamingData.Models;
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoParleyBetSlipData : IMongoParleyBetSlipData
{
    private readonly IMongoCollection<ParleyBetSlipModel> _parleyBetSlips;
    private readonly IMongoDbConnection _database;
    private readonly ILogger<MongoParleyBetSlipData> _logger;
    private readonly IMongoUserData _userData;

    public MongoParleyBetSlipData(
                                  IMongoDbConnection mongoDbConnection,
                                  ILogger<MongoParleyBetSlipData> logger,
                                  IMongoUserData userData)
    {
        _parleyBetSlips = mongoDbConnection.ParleyBetSlipsCollection;
        _logger = logger;
        _userData = userData;
        _database = mongoDbConnection;
    }

    public async Task CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip)
    {
        _logger.LogInformation("Calling Create Parley Bet Slip / MongoParleyBetSlipData");

        var client = _database.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_database.DatabaseName);

            var usersInTransaction = db.GetCollection<UserModel>(
                _database.UsersCollectionName);

            var user = await _userData.GetCurrentUserByUserId(parleyBetSlip.BettorId);

            user.AccountBalance -= parleyBetSlip.ParleyBetAmount;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == parleyBetSlip.BettorId, user);

            await _parleyBetSlips.InsertOneAsync(parleyBetSlip);

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Parley Bet...Transaction Aborted / ParleyBetSlipData");

            await session.AbortTransactionAsync();
        }
    }

    public async Task<List<ParleyBetSlipModel>> GetBettorParleyBetSlips(string userId)
    {
        var results = await _parleyBetSlips.FindAsync(b => b.BettorId == userId);

        return results.ToList();
    }

    public async Task<List<ParleyBetSlipModel>> GetAllParleyBetSlipsInProgress()
    {
        var results = await _parleyBetSlips.FindAsync(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.IN_PROGRESS);

        return results.ToList();
    }

    public async Task UpdateParleyBetSlip(ParleyBetSlipModel parleyBetSlip)
    {
        _logger.LogInformation("Calling Update Parley Bet Slip / MongoParleyBetSlipData");

        var client = _database.Client;

        using var session = await client.StartSessionAsync();

        session.StartTransaction();
        try
        {
            var db = client.GetDatabase(_database.DatabaseName);

            var usersInTransaction = db.GetCollection<UserModel>(
                _database.UsersCollectionName);

            var user = await _userData.GetCurrentUserByUserId(parleyBetSlip.BettorId);

            user.AccountBalance =
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER ?
                    user.AccountBalance + parleyBetSlip.ParleyBetPayout :
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH ?
                    user.AccountBalance + parleyBetSlip.ParleyBetAmount
                    : user.AccountBalance;

            if (parleyBetSlip.ParleyBetSlipStatus != ParleyBetSlipStatus.LOSER)
                parleyBetSlip.ParleyBetSlipPayoutStatus = ParleyBetSlipPayoutStatus.PAID;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == parleyBetSlip.BettorId, user);

            var filter = Builders<ParleyBetSlipModel>.Filter.Eq(
            "ParleyBetSlipId", parleyBetSlip.ParleyBetSlipId);

            await _parleyBetSlips.ReplaceOneAsync(
                filter, parleyBetSlip, new ReplaceOptions { IsUpsert = true });

            await session.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Update Parley Bet...Transaction Aborted / ParleyBetSlipData");

            await session.AbortTransactionAsync();
        }
    }
}
