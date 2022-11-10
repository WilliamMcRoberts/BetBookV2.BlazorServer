
namespace BetBookGamingData.Data;

public class MongoHouseAccountData : IMongoHouseAccountData
{
    private readonly IMongoCollection<HouseAccountModel> _house;
    private readonly IConfiguration _config;

    public MongoHouseAccountData(IMongoDbConnection mongoDbConnection, IConfiguration config)
    {
        _house = mongoDbConnection.HouseAccountCollection;
        _config = config;
    }

    public async Task CreateHouseAccount()
    {
        await _house.InsertOneAsync(new HouseAccountModel() { HouseAccountBalance = 500000 });
    }

    public async Task<HouseAccountModel> GetHouseAccount()
    {
        
        var house = await _house.FindAsync(h => h.HouseId == _config.GetSection("HouseAccount:HouseId").Value);

        return house.FirstOrDefault();
    }

    public Task UpdateHouseAccount(HouseAccountModel house)
    {

        var filter = Builders<HouseAccountModel>.Filter.Eq(
            "HouseId", house.HouseId);

        return _house.ReplaceOneAsync(
            filter, house, new ReplaceOptions { IsUpsert = true });
    }
}
