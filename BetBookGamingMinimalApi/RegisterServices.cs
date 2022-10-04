using BetBookGamingData.Data;
using BetBookGamingData.DbAccess;
using BetBookGamingData.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Serilog;


namespace BetBookGamingMinimalApi;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
                        .EnableTokenAcquisitionToCallDownstreamApi()
                        .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
                        .AddInMemoryTokenCaches()
                        .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
                        .AddInMemoryTokenCaches();
        builder.Services.AddAuthorization();

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
