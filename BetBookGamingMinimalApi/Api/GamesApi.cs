using BetBookGamingData;
using BetBookGamingData.Services;

namespace BetBookGamingMinimalApi.Api;

public static class GamesApi
{
    public static void ConfigureGamesApi(this WebApplication app)
    {
        app.MapGet("/Games/{season}/{week}", GetGamesByWeekAndSeason)
            .WithName("GetGames").RequireAuthorization();
    }

    private static async Task<IResult> GetGamesByWeekAndSeason(
        SeasonType season, int week, IGameService gameService)
    {
        try
        {
            return Results.Ok(await gameService.GetGamesByWeek(season, week));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
