
namespace BetBookGamingData.Data;

public class MongoParleyBetSlipData : IMongoParleyBetSlipData
{
    private readonly IMongoCollection<ParleyBetSlipModel> _parleyBetSlips;
    private readonly IMongoDbConnection _database;
    private readonly ILogger<MongoParleyBetSlipData> _logger;
    private readonly IMongoUserData _userData;
    private readonly IConfiguration _config;
    private readonly IMongoHouseAccountData _houseData;


    public MongoParleyBetSlipData(
                                  IMongoDbConnection mongoDbConnection,
                                  ILogger<MongoParleyBetSlipData> logger,
                                  IMongoUserData userData,
                                  IConfiguration config,
                                  IMongoHouseAccountData houseData)
    {
        _parleyBetSlips = mongoDbConnection.ParleyBetSlipsCollection;
        _logger = logger;
        _userData = userData;
        _database = mongoDbConnection;
        _config = config;
        _houseData = houseData;
    }

    public async Task<bool> CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip)
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

            var houseAccountInTransaction = db.GetCollection<HouseAccountModel>(
                _database.HouseAccountCollectionName);

            var user = await _userData.GetCurrentUserByUserId(parleyBetSlip.BettorId);
            var house = await _houseData.GetHouseAccount();

            user.AccountBalance -= parleyBetSlip.ParleyBetAmount;
            house.HouseAccountBalance += parleyBetSlip.ParleyBetAmount;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == parleyBetSlip.BettorId, user);

            await houseAccountInTransaction.ReplaceOneAsync(
                session, h => h.HouseId == _config.GetSection("HouseAccount:HouseId").Value, house);

            await _parleyBetSlips.InsertOneAsync(parleyBetSlip);

            await session.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, message: "Failed To Insert Parley Bet...Transaction Aborted / ParleyBetSlipData");

            await session.AbortTransactionAsync();
            return false;
        }
    }

    public async Task<List<ParleyBetSlipModel>> GetBettorParleyBetSlips(string userId)
    {
        _logger.LogInformation("Calling GetBettorParleyBetSlips / MongoParleyBetSlipData");

        var bets = new List<ParleyBetSlipModel>();
        try
        {
            var results = await _parleyBetSlips.FindAsync(b => b.BettorId == userId);
            bets = results.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, message: "Failed To Get Bettor Parley Bets / ParleyBetSlipData");
        }

        return bets;
    }

    public async Task<List<ParleyBetSlipModel>> GetAllParleyBetSlipsInProgress()
    {
        var bets = new List<ParleyBetSlipModel>();
        try
        {
            var results = await _parleyBetSlips.FindAsync(b => b.ParleyBetSlipStatus == ParleyBetSlipStatus.IN_PROGRESS);
            bets = results.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, message: "Failed To Get Parley Bets In Progress / ParleyBetSlipData");
        }

        return bets;
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

            var houseAccountInTransaction = db.GetCollection<HouseAccountModel>(
                _database.HouseAccountCollectionName);

            var user = await _userData.GetCurrentUserByUserId(parleyBetSlip.BettorId);
            var house = await _houseData.GetHouseAccount();

            user.AccountBalance =
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER ?
                    user.AccountBalance + parleyBetSlip.ParleyBetPayout :
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH ?
                    user.AccountBalance + parleyBetSlip.ParleyBetAmount
                    : user.AccountBalance;

            house.HouseAccountBalance =
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.WINNER ?
                    house.HouseAccountBalance - parleyBetSlip.ParleyBetPayout :
                parleyBetSlip.ParleyBetSlipStatus == ParleyBetSlipStatus.PUSH ?
                    house.HouseAccountBalance - parleyBetSlip.ParleyBetAmount
                    : house.HouseAccountBalance;

            if (parleyBetSlip.ParleyBetSlipStatus != ParleyBetSlipStatus.LOSER)
                parleyBetSlip.ParleyBetSlipPayoutStatus = ParleyBetSlipPayoutStatus.PAID;

            await usersInTransaction.ReplaceOneAsync(
                session, u => u.UserId == parleyBetSlip.BettorId, user);

            await houseAccountInTransaction.ReplaceOneAsync(
                session, h => h.HouseId == _config.GetSection("HouseAccount:HouseId").Value, house);

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
