using Microsoft.AspNetCore.Components.Authorization;

namespace BetBookGamingUserInterface.Helpers;

public static class AuthenticationStateProviderHelper
{
    public static async Task<UserModel> GetUserFromAuth(
  this AuthenticationStateProvider provider, IMongoUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(
            c => c.Type.Contains("objectidentifier"))?.Value;
        return await userData.GetCurrentUserFromAuthentication(objectId);
    }
}
