using BetBookGamingData.Data;
using BetBookGamingData.Models;

namespace BetBookGamingMinimalApi.Api;

public static class SingleBetsApi
{
    public static void ConfigureSingleBetsApi(this WebApplication app)
    {
        app.MapGet("/SingleBets/BettorId/{bettorId}", GetBettorSingleBets)
            .WithName("GetBettorSingleBets");
        app.MapGet("/SingleBets/ScoreIdOfGame/{scoreIdOfGame}", GetAllSingleBetsOnGameInProgress)
            .WithName("GetAllSingleBetsOnGameInProgress");
        app.MapPost("/SingleBets", CreateSingleBet)
            .WithName("CreateSingleBet");
        app.MapPut("/SingleBets", UpdateSingleBet)
            .WithName("UpdateSingleBet");
    }

    private static async Task<IResult> GetBettorSingleBets(
        string bettorId, IMongoSingleBetData singleBetData)
    {
        try
        {
            return Results.Ok(
                await singleBetData.GetBettorSingleBets(bettorId));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetAllSingleBetsOnGameInProgress(
        int scoreIdOfGame, IMongoSingleBetData singleBetData)
    {
        try
        {
            return Results.Ok(
                await singleBetData.GetAllSingleBetsOnGameInProgress(scoreIdOfGame));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> CreateSingleBet(
        SingleBetModel singleBet, IMongoSingleBetData singleBetData)
    {
        try
        {
            await singleBetData.CreateSingleBet(singleBet);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateSingleBet(
        SingleBetModel singleBet, IMongoSingleBetData singleBetData)
    {
        try
        {
            await singleBetData.UpdateSingleBet(singleBet);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
