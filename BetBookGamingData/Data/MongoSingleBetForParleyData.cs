
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
}
