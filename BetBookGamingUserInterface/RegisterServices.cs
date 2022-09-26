using BetBookGamingData.Data;
using BetBookGamingData.DbAccess;
using BetBookGamingData.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Serilog;
using Syncfusion.Blazor;

namespace BetBookGamingUserInterface;


public static class RegisterServices
{

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        /******************* Microsoft Authentication ***********************/

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

        /********************** Admin Authorization ***************************/

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("JobTitle", "Admin");
            });
        });

        /************************** Add / Use *********************************/

        builder.Host.UseSerilog();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
        builder.Services.AddMemoryCache();
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddMemoryCache();

        /************************ Hosted Services *****************************/

        builder.Services.AddHostedService<UpdateSingleBetsTimer>();
        builder.Services.AddHostedService<UpdateParleyBetsTimer>();

        /********************** Application State *****************************/

        builder.Services.AddScoped<BetSlipState>();

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