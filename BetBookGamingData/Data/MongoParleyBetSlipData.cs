
using BetBookGamingData.Models;
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoParleyBetSlipData : IMongoParleyBetSlipData
{
    private readonly IMongoCollection<ParleyBetSlipModel> _parleyBetSlips;
    private readonly ILogger<MongoSingleBetForParleyData> _logger;
    private readonly IMongoUserData _userData;

    public MongoParleyBetSlipData(
        IMongoDbConnection mongoDbConnection, ILogger<MongoSingleBetForParleyData> logger, IMongoUserData userData)
    {
        _parleyBetSlips = mongoDbConnection.ParleyBetSlipsCollection;
        _logger = logger;
        _userData = userData;
    }

    public Task CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip, UserModel user)
    {
        _logger.LogInformation("Calling Create Parley Bet Slip / MongoParleyBetSlipData");

        user.AccountBalance -= parleyBetSlip.ParleyBetAmount;

        _userData.UpdateUser(user);
        return _parleyBetSlips.InsertOneAsync(parleyBetSlip);
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

        UserModel user = await _userData.GetCurrentUserByUserId(parleyBetSlip.BettorId);

        user.AccountBalance =
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER ?
                    user.AccountBalance + parleyBetSlip.ParleyBetPayout :
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH ?
                    user.AccountBalance + parleyBetSlip.ParleyBetAmount
                    : user.AccountBalance;

        if (parleyBetSlip.ParleyBetSlipStatus != ParleyBetSlipStatus.LOSER)
            parleyBetSlip.ParleyBetSlipPayoutStatus = ParleyBetSlipPayoutStatus.PAID;

        await _userData.UpdateUser(user);


        var filter = Builders<ParleyBetSlipModel>.Filter.Eq(
            "ParleyBetSlipId", parleyBetSlip.ParleyBetSlipId);

        await _parleyBetSlips.ReplaceOneAsync(
            filter, parleyBetSlip, new ReplaceOptions { IsUpsert = true });
    }
}
