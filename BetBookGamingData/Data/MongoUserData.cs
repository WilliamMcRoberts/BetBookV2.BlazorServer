
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Data;

public class MongoUserData : IMongoUserData
{
	private readonly IMongoCollection<UserModel> _users;
	private readonly ILogger<IMongoUserData> _logger;

	public MongoUserData(
		IMongoDbConnection mongoDbConnection, ILogger<IMongoUserData> logger)
	{
		_users = mongoDbConnection.UsersCollection;
		_logger = logger;
	}

	public async Task<UserModel> GetCurrentUserByUserId(string userId)
	{
		_logger.LogInformation(
			"Calling Get Current User By User Id / MongoUserData");

		var users = await _users.FindAsync(u => u.UserId == userId);

		return users.FirstOrDefault();
	}

	public async Task<UserModel> GetCurrentUserFromAuthentication(string objectId)
	{
		_logger.LogInformation(
			"Calling Get Current User From Authentication(ObjectId) / MongoUserData");

		var users = await _users.FindAsync(u => u.ObjectIdentifier == objectId);

		return users.FirstOrDefault();
	}

	public Task CreateUser(UserModel user)
	{
        _logger.LogInformation("Calling Create User / MongoUserData");

        return _users.InsertOneAsync(user);
	}

	public Task UpdateUser(UserModel user)
	{
        _logger.LogInformation("Calling Update User / MongoUserData");

        var filter = Builders<UserModel>.Filter.Eq(
			"UserId", user.UserId);

		return _users.ReplaceOneAsync(
			filter, user, new ReplaceOptions { IsUpsert = true });
	}
}
