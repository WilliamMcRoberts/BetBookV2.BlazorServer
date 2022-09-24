
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoSingleBetData : IMongoSingleBetData
{
    private readonly IMongoCollection<SingleBetModel> _singleBets;
    private readonly IMongoDbConnection _database;
    private readonly IMongoUserData _userData;
    private readonly IMongoHouseAccountData _houseData;
    private readonly ILogger<MongoSingleBetData> _logger;
    private readonly IConfiguration _config;


    public MongoSingleBetData(IMongoDbConnection mongoDbConnection,
                              ILogger<MongoSingleBetData> logger,
                              IMongoUserData userData,
                              IMongoHouseAccountData houseData,
                              IConfiguration config)
    {
        _singleBets = mongoDbConnection.SingleBetsCollection;
        _logger = logger;
        _userData = userData;
        _database = mongoDbConnection;
        _houseData = houseData;
        _config = config;
    }

    public async Task<bool> CreateSingleBet(SingleBetModel singleBet)
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

            var houseAccountInTransaction = db.GetCollection<HouseAccountModel>(
                _database.HouseAccountCollectionName);

            var user = await _userData.GetCurrentUserByUserId(singleBet.BettorId);
            var house = await _houseData.GetHouseAccount();

            user.AccountBalance -= singleBet.BetAmount;
            house.HouseAccountBalance += singleBet.BetAmount;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == singleBet.BettorId, user);

            await houseAccountInTransaction.ReplaceOneAsync(
                session, h => h.HouseId == _config.GetSection("HouseAccount:HouseId").Value, house);

            await _singleBets.InsertOneAsync(singleBet);

            await session.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Insert Single Bet...Transaction Aborted / SingleBetData");

            await session.AbortTransactionAsync();
            return false;
        }
    }

    public async Task<List<SingleBetModel>> GetBettorSingleBets(string bettorId)
    {
        var results = await _singleBets.FindAsync(b => b.BettorId == bettorId);

        return results.ToList();
    }

    public async Task<List<SingleBetModel>> GetAllSingleBetsOnGameInProgress(int scoreIdOfGame)
    {
        var results =
            await _singleBets.FindAsync(
                b => b.GameSnapshot.ScoreID == scoreIdOfGame 
                && b.SingleBetStatus == SingleBetStatus.IN_PROGRESS);

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

            var houseAccountInTransaction = db.GetCollection<HouseAccountModel>(
                _database.HouseAccountCollectionName);

            var user = await _userData.GetCurrentUserByUserId(singleBet.BettorId);
            var house = await _houseData.GetHouseAccount();

            user.AccountBalance =
                singleBet.SingleBetStatus == SingleBetStatus.WINNER ?
                    user.AccountBalance + singleBet.BetPayout :
                singleBet.SingleBetStatus == SingleBetStatus.PUSH ?
                    user.AccountBalance + singleBet.BetAmount
                    : user.AccountBalance;

            house.HouseAccountBalance =
                singleBet.SingleBetStatus == SingleBetStatus.WINNER ?
                    house.HouseAccountBalance - singleBet.BetPayout :
                singleBet.SingleBetStatus == SingleBetStatus.PUSH ?
                    house.HouseAccountBalance - singleBet.BetAmount
                    : house.HouseAccountBalance;

            if (singleBet.SingleBetStatus != SingleBetStatus.LOSER)
                singleBet.SingleBetPayoutStatus = SingleBetPayoutStatus.PAID;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == singleBet.BettorId, user);

            await houseAccountInTransaction.ReplaceOneAsync(
                session, h => h.HouseId == _config.GetSection("HouseAccount:HouseId").Value, house);

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
