using BetBookGamingData.Services;
using BetBookGamingData;
using BetBookGamingData.Data;
using BetBookGamingData.Models;

namespace BetBookGamingMinimalApi.Api;

public static class UsersApi
{
    public static void ConfigureUsersApi(this WebApplication app)
    {
        app.MapPost("/Users", CreateUser)
            .WithName("CreateUser");
        app.MapPut("/Users", UpdateUser)
            .WithName("UpdateUser");
        app.MapGet("/Users/ObjectId/{objectId}", GetCurrentUserFromAuthentication)
            .WithName("GetUserFromAuthentication");
        app.MapGet("/Users/UserId/{userId}", GetCurrentUserByUserId)
            .WithName("GetUserByUserId");
    }

    private static async Task<IResult> GetCurrentUserByUserId(
        string userId, IMongoUserData userData)
    {
        try
        {
            return Results.Ok(
                await userData.GetCurrentUserByUserId(userId));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetCurrentUserFromAuthentication(
        string objectId, IMongoUserData userData)
    {
        try
        {
            return Results.Ok(
                await userData.GetCurrentUserFromAuthentication(objectId));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> CreateUser(
        UserModel user, IMongoUserData userData)
    {
        try
        {
            await userData.CreateUser(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUser(
        UserModel user, IMongoUserData userData)
    {
        try
        {
            await userData.UpdateUser(user);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
