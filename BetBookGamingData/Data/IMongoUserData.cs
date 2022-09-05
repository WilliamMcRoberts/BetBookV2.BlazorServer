namespace BetBookGamingData.Data
{
    public interface IMongoUserData
    {
        Task CreateUser(UserModel user);
        Task<UserModel> GetCurrentUserByUserId(string userId);
        Task<UserModel> GetCurrentUserFromAuthentication(string objectId);
        Task UpdateUser(UserModel user);
    }
}