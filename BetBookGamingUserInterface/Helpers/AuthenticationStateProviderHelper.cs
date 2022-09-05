using Microsoft.AspNetCore.Components.Authorization;

namespace BetBookGamingUserInterface.Helpers;

#nullable disable

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

    public static async Task<UserModel> LoadAndVerifyUser(
        this AuthenticationStateProvider provider, IMongoUserData userData)
    {
        var authState = await provider.GetAuthenticationStateAsync();
        string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
        UserModel loggedInUser = await userData.GetCurrentUserFromAuthentication(objectId) ?? new();

        if (string.IsNullOrWhiteSpace(objectId) == false)
        {
            string firstName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("givenname"))?.Value;
            string lastName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value;
            string displayName = authState.User.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
            string emailAddress = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value;
            bool isDirty = false;
            if (objectId.Equals(loggedInUser.ObjectIdentifier) == false)
            {
                isDirty = true;
                loggedInUser.ObjectIdentifier = objectId;
            }

            if (firstName.Equals(loggedInUser.FirstName) == false)
            {
                isDirty = true;
                loggedInUser.FirstName = firstName;
            }

            if (lastName.Equals(loggedInUser.LastName) == false)
            {
                isDirty = true;
                loggedInUser.LastName = lastName;
            }

            if (displayName.Equals(loggedInUser.DisplayName) == false)
            {
                isDirty = true;
                loggedInUser.DisplayName = displayName;
            }

            if (emailAddress.Equals(loggedInUser.EmailAddress) == false)
            {
                isDirty = true;
                loggedInUser.EmailAddress = emailAddress;
            }

            if (isDirty)
            {
                if (string.IsNullOrWhiteSpace(loggedInUser.UserId))
                {
                    loggedInUser.AccountBalance = 10000;

                    await userData.CreateUser(loggedInUser);
                }
                else
                {
                    await userData.UpdateUser(loggedInUser);
                }
            }
        }

        return loggedInUser;
    }
}

#nullable enable