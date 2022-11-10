
namespace BetBookGamingUserInterface.Helpers;

public static class AuthenticationStateProviderHelper
{
    public static async Task<UserModel> GetUserFromAuth(
        this Task<AuthenticationState> authStateTask, IMongoUserData userData)
    {
        var authState = await authStateTask;
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;
        return await userData.GetCurrentUserFromAuthentication(objectId);
    }
}

