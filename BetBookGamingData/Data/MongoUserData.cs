
namespace BetBookGamingData.Data;

public class MongoUserData : IMongoUserData
{
	private readonly IMongoCollection<UserModel> _users;
	private readonly ILogger<IMongoUserData> _logger;
	private readonly IConfiguration _configuration;

	public MongoUserData(IMongoDbConnection mongoDbConnection, 
		ILogger<IMongoUserData> logger, IConfiguration configuration)
	{
		_users = mongoDbConnection.UsersCollection;
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<UserModel> GetCurrentUserByUserId(string userId)
	{
		_logger.LogInformation("Calling Get Current User By User Id / MongoUserData");

		var users = await _users.FindAsync(u => u.UserId == userId);

		return users.FirstOrDefault();
	}

	public async Task<UserModel> GetCurrentUserFromAuthentication(string objectId)
	{
		_logger.LogInformation("Calling Get Current User From Authentication(ObjectId) / MongoUserData");

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

        var filter = Builders<UserModel>.Filter.Eq("UserId", user.UserId);

		return _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
	}

	public async Task<string> MakeStripePayment(string cardNumber, string expirationMonth, string expirationYear, string cvc, decimal value)
	{
		try
		{
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe:SecretKey").Value;

            var optionToken = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = cardNumber,
                    ExpMonth = expirationMonth,
                    ExpYear = expirationYear,
                    Cvc = cvc
                }
            };

			var serviceToken = new TokenService();

			Token stripeToken = await serviceToken.CreateAsync(optionToken);

			var customer = new Customer
			{
				Name = "",
				Address = new Address
				{
					Country = "",
					City = "",
					Line1 = "",
					PostalCode = ""
				}
			};

			var options = new ChargeCreateOptions
			{
				Amount = Convert.ToInt32(value),
				Currency = "USD",
				Description = "Test",
				Source = stripeToken.Id
			};

			var service = new ChargeService();

			Charge charge = await service.CreateAsync(options);

            return charge.Paid ? "Stripe payment successful" : "Stripe payment failed";
        }
        catch (Exception ex)
		{
			_logger.LogInformation(ex.Message, "Stripe Payment Failed / UserData");
		}

		return null;
    }
}
