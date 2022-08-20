using BetBookGamingData.Models;

namespace BetBookGamingData.Interfaces;

#nullable enable

/// <summary>
/// HouseAccountData interface
/// </summary>
public interface IHouseAccountData
{
    Task<HouseAccountModel?> GetHouseAccount();
    Task UpdateHouseAccount(HouseAccountModel houseAccount);
}

#nullable restore
