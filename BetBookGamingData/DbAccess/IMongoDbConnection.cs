using BetBookGamingData.Models;
using MongoDB.Driver;

namespace BetBookGamingData.DbAccess
{
    public interface IMongoDbConnection
    {
        MongoClient Client { get; }
        string DatabaseName { get; }
        IMongoCollection<GameModel> GamesCollection { get; }
        string GamesCollectionName { get; }
        IMongoCollection<ParleyBetSlipModel> ParleyBetSlipsCollection { get; }
        string ParleyBetSlipsCollectionName { get; }
        IMongoCollection<SingleBetModel> SingleBetsCollection { get; }
        string SingleBetsCollectionName { get; }
        IMongoCollection<SingleBetForParleyModel> SingleBetsForParleyCollection { get; }
        string SingleBetsForParleyCollectionName { get; }
        IMongoCollection<TeamModel> TeamsCollection { get; }
        string TeamsCollectionName { get; }
        IMongoCollection<UserModel> UsersCollection { get; }
        string UsersCollectionName { get; }
    }
}