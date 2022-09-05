
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoSingleBetData : IMongoSingleBetData
{
    private readonly IMongoCollection<SingleBetModel> _singleBets;
    private readonly ILogger<MongoSingleBetData> _logger;

    public MongoSingleBetData(
        IMongoDbConnection mongoDbConnection, ILogger<MongoSingleBetData> logger)
    {
        _singleBets = mongoDbConnection.SingleBetsCollection;
        _logger = logger;
    }

    public Task CreateSingleBet(SingleBetModel singleBet)
    {
        _logger.LogInformation("Calling Create Single Bet / MongoSingleBetData");

         return _singleBets.InsertOneAsync(singleBet);
    }
}
