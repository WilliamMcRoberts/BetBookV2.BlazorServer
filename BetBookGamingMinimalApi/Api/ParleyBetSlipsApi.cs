using BetBookGamingData.Data;
using BetBookGamingData.Models;

namespace BetBookGamingMinimalApi.Api;

public static class ParleyBetSlipsApi
{
    public static void ConfigureParleyBetSlipsApi(this WebApplication app)
    {
        app.MapGet("/ParleyBetSlips/{bettorId}", GetBettorParleyBetSlips)
            .WithName("GetBettorParleyBetSlips");
        app.MapGet("/ParleyBetSlips/InProgress", GetAllParleyBetSlipsInProgress)
            .WithName("GetAllParleyBetSlipsInProgress");
        app.MapPost("/ParleyBetSlips", CreateParleyBetSlip)
            .WithName("CreateParleyBetSlip");
        app.MapPut("/ParleyBetSlips", UpdateParleyBetSlip)
            .WithName("UpdateParleyBetSlips");
    }

    private static async Task<IResult> GetBettorParleyBetSlips(
        string bettorId, IMongoParleyBetSlipData parleyBetSlipData)
    {
        try
        {
            return Results.Ok(
                await parleyBetSlipData.GetBettorParleyBetSlips(bettorId));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetAllParleyBetSlipsInProgress(
        IMongoParleyBetSlipData parleyBetSlipData)
    {
        try
        {
            return Results.Ok(
                await parleyBetSlipData.GetAllParleyBetSlipsInProgress());
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> CreateParleyBetSlip(
        ParleyBetSlipModel parleyBetSlip, IMongoParleyBetSlipData parleyBetSlipData)
    {
        try
        {
            await parleyBetSlipData.CreateParleyBetSlip(parleyBetSlip);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateParleyBetSlip(
        ParleyBetSlipModel parleyBetSlip, IMongoParleyBetSlipData parleyBetSlipData)
    {
        try
        {
            await parleyBetSlipData.UpdateParleyBetSlip(parleyBetSlip);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
