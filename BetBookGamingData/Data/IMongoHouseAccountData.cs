namespace BetBookGamingData.Data;

public interface IMongoHouseAccountData
{
    Task CreateHouseAccount();
    Task<HouseAccountModel> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel house);
}