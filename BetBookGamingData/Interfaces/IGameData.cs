using BetBookGamingData.Models;

namespace BetBookGamingData.Interfaces;

#nullable enable

public interface IGameData
{
    Task DeleteGame(int id);
    Task<GameModel?> GetGame(int id);
    Task<IEnumerable<GameModel>> GetGames();
    Task InsertGame(GameModel game);
    Task UpdateGame(GameModel game);
}

#nullable restore
