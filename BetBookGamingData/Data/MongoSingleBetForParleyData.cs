
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoSingleBetForParleyData : IMongoSingleBetForParleyData
{
    private readonly IMongoCollection<SingleBetForParleyModel> _singleBetsForParley;
    private readonly ILogger<MongoSingleBetForParleyData> _logger;

    public MongoSingleBetForParleyData(
        IMongoDbConnection mongoDbConnection, ILogger<MongoSingleBetForParleyData> logger)
    {
        _singleBetsForParley = mongoDbConnection.SingleBetsForParleyCollection;
        _logger = logger;
    }

    public Task CreateSingleBetForParley(SingleBetForParleyModel singleBetForParley)
    {
        _logger.LogInformation("Calling Create Single Bet For Parley / MongoSingleBetForParleyData");

        return _singleBetsForParley.InsertOneAsync(singleBetForParley);
    }

    public async Task<List<SingleBetForParleyModel>> GetAllSingleBetsForParleyOnGameInProgress(int scoreIdOfGame)
    {
        var results =
            await _singleBetsForParley.FindAsync(
                b => b.ScoreIdOfGame == scoreIdOfGame && b.SingleBetForParleyStatus == SingleBetForParleyStatus.IN_PROGRESS);

        return results.ToList();
    }

    public async Task UpdateSingleBetForParley(SingleBetForParleyModel singleBetForParley)
    {
        _logger.LogInformation("Calling Update Single Bet For Parley / MongoSingleBetForParleyData");

        var filter = Builders<SingleBetForParleyModel>.Filter.Eq(
            "SingleBetForParleyId", singleBetForParley.SingleBetForParleyId);

        await _singleBetsForParley.ReplaceOneAsync(
            filter, singleBetForParley, new ReplaceOptions { IsUpsert = true });
    }
}
