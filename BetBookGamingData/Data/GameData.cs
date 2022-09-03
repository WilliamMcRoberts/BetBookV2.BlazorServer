using BetBookGamingData.DbAccess;
using BetBookGamingData.Models;

namespace BetBookGamingData.Data;

#nullable enable


public interface IGameData
{
    Task DeleteGame(int id);
    Task<GameModel?> GetGame(int id);
    Task<IEnumerable<GameModel>> GetGames();
    Task InsertGame(GameModel game);
    Task UpdateGame(GameModel game);
}


public class GameData : IGameData
{
    private readonly ISqlConnection _db;

    public GameData(ISqlConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<GameModel>> GetGames() => 
        await _db.LoadData<GameModel, dynamic>("dbo.spGames_GetAll", new{});

    public async Task<GameModel?> GetGame(int id)
    {
        var results = await _db.LoadData<GameModel, dynamic>(
            "dbo.spGames_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    public async Task InsertGame(GameModel game)
    {
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Insert", new
        {
            game.HomeTeamId,
            game.AwayTeamId,
            game.Stadium,
            game.PointSpread,
            game.WeekNumber,
            seasonType,
            game.DateOfGame,
            gameStatus,
            game.ScoreId,
            game.DateOfGameOnly,
            game.TimeOfGameOnly
        });
    }

    public async Task UpdateGame(GameModel game)
    {
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Update", new
        {
            game.Id,
            game.HomeTeamId,
            game.AwayTeamId,
            game.Stadium,
            game.PointSpread,
            game.HomeTeamFinalScore,
            game.AwayTeamFinalScore,
            game.GameWinnerId,
            seasonType,
            game.DateOfGame,
            gameStatus,
            game.ScoreId,
            game.DateOfGameOnly,
            game.TimeOfGameOnly
        });
    }

    public async Task DeleteGame(int id)
    {
        await _db.SaveData(
        "dbo.spGames_Delete", new
        {
            Id = id
        });
    }
}


#nullable restore

