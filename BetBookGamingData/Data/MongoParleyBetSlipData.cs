
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
}
