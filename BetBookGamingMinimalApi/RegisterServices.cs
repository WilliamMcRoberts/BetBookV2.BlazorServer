using BetBookGamingData.Data;
using BetBookGamingData.DbAccess;
using BetBookGamingData.Services;
using Serilog;


namespace BetBookGamingMinimalApi;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /*************************** Services *********************************/

        builder.Services.AddSingleton<IGameService, GameService>();

        /*********************** Http Client Factory **************************/

        builder.Services.AddHttpClient("sportsdata", client =>
        {
            client.BaseAddress = new Uri("https://api.sportsdata.io/v3/nfl/");
        });

        /*************************** Data access ******************************/

        builder.Services.AddSingleton<IMongoDbConnection, MongoDbConnection>();
        builder.Services.AddTransient<IMongoUserData, MongoUserData>();
        builder.Services.AddTransient<IMongoSingleBetData, MongoSingleBetData>();
        builder.Services.AddTransient<IMongoParleyBetSlipData, MongoParleyBetSlipData>();
        builder.Services.AddTransient<IMongoHouseAccountData, MongoHouseAccountData>();
    }
}
