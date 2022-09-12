

namespace BetBookGamingData.DbAccess;

public class MongoDbConnection : IMongoDbConnection
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly string _connectionId = "MongoDatabase";

    public string DatabaseName { get; private set; }

    public string UsersCollectionName { get; private set; } = "users";
    public string TeamsCollectionName { get; private set; } = "teams";
    public string SingleBetsCollectionName { get; private set; } = "single-bets";
    public string SingleBetsForParleyCollectionName { get; private set; } = "single-bets-for-parley";
    public string ParleyBetSlipsCollectionName { get; private set; } = "parley-bet-slips";
    public string GamesCollectionName { get; private set; } = "games";
    public string HouseAccountCollectionName { get; private set; } = "house-account";
    public MongoClient Client { get; private set; }
    public IMongoCollection<UserModel> UsersCollection { get; private set; }
    public IMongoCollection<TeamModel> TeamsCollection { get; private set; }
    public IMongoCollection<SingleBetModel> SingleBetsCollection { get; private set; }
    public IMongoCollection<SingleBetForParleyModel> SingleBetsForParleyCollection { get; private set; }
    public IMongoCollection<ParleyBetSlipModel> ParleyBetSlipsCollection { get; private set; }
    public IMongoCollection<GameModel> GamesCollection { get; private set; }
    public IMongoCollection<HouseAccountModel> HouseAccountCollection { get; private set; }

    public MongoDbConnection(IConfiguration configuration)
    {
        _configuration = configuration;
        Client = new MongoClient(_configuration.GetConnectionString(_connectionId));
        DatabaseName = _configuration["DatabaseName"];
        _mongoDatabase = Client.GetDatabase(DatabaseName);

        UsersCollection = _mongoDatabase.GetCollection<UserModel>(UsersCollectionName);
        TeamsCollection = _mongoDatabase.GetCollection<TeamModel>(TeamsCollectionName);
        SingleBetsCollection = _mongoDatabase.GetCollection<SingleBetModel>(SingleBetsCollectionName);
        SingleBetsForParleyCollection = _mongoDatabase.GetCollection<SingleBetForParleyModel>(SingleBetsForParleyCollectionName);
        ParleyBetSlipsCollection = _mongoDatabase.GetCollection<ParleyBetSlipModel>(ParleyBetSlipsCollectionName);
        GamesCollection = _mongoDatabase.GetCollection<GameModel>(GamesCollectionName);
        HouseAccountCollection = _mongoDatabase.GetCollection<HouseAccountModel>(HouseAccountCollectionName);
    }
}
