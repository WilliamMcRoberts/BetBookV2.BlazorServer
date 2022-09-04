
namespace BetBookGamingData.Data;

public class MongoUserData : IMongoUserData
{
	private readonly IMongoCollection<UserModel> _users;

	public MongoUserData(IMongoDbConnection mongoDbConnection)
	{
		_users = mongoDbConnection.UsersCollection;
	}

	public async Task<UserModel> GetCurrentUser(string userId)
	{
		var users = await _users.FindAsync(u => u.UserId == userId);

		return users.FirstOrDefault();
	}

	public async Task<UserModel> GetCurrentUserFromAuthentication(string objectId)
	{
		var users = await _users.FindAsync(u => u.ObjectIdentifier == objectId);

		return users.FirstOrDefault();
	}

	public Task CreateUser(UserModel user)
	{
		return _users.InsertOneAsync(user);
	}

	public Task UpdateUser(UserModel user)
	{
		var filter = Builders<UserModel>.Filter.Eq(
			"UserId", user.UserId);

		return _users.ReplaceOneAsync(
			filter, user, new ReplaceOptions { IsUpsert = true });
	}
}
