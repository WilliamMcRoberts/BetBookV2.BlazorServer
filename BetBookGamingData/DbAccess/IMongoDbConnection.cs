namespace BetBookGamingData.DbAccess
{
    public interface IMongoDbConnection
    {
        MongoClient Client { get; }
        string DatabaseName { get; }
        IMongoCollection<HouseAccountModel> HouseAccountCollection { get; }
        string HouseAccountCollectionName { get; }
        IMongoCollection<ParleyBetSlipModel> ParleyBetSlipsCollection { get; }
        string ParleyBetSlipsCollectionName { get; }
        IMongoCollection<SingleBetModel> SingleBetsCollection { get; }
        string SingleBetsCollectionName { get; }
        IMongoCollection<SingleBetForParleyModel> SingleBetsForParleyCollection { get; }
        string SingleBetsForParleyCollectionName { get; }
        IMongoCollection<UserModel> UsersCollection { get; }
        string UsersCollectionName { get; }
    }
}