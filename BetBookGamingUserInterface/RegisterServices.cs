
using BetBookGamingUserInterface.Components;

namespace BetBookGamingUserInterface;

public static class RegisterServices
{

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        /******************* Authentication ***********************/

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

        /********************** Authorization ***************************/

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("JobTitle", "Admin");
            });
        });

        builder.Host.UseSerilog();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();
        builder.Services.AddMemoryCache();
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
        builder.Services.AddSyncfusionBlazor();

        builder.Services.AddHostedService<UpdateSingleBetsTimer>();
        builder.Services.AddHostedService<UpdateParleyBetsTimer>();
        StripeConfiguration.ApiKey = builder.Configuration[key: "Stripe:SecretKey"];

        builder.Services.AddSingleton<IGameService, GameService>();

        builder.Services.AddHttpClient("sportsdata", client =>
        {
            client.BaseAddress = new Uri("https://api.sportsdata.io/v3/nfl/");
        });


        builder.Services.AddSingleton<IMongoDbConnection, MongoDbConnection>();
        builder.Services.AddTransient<IMongoUserData, MongoUserData>();
        builder.Services.AddTransient<IMongoSingleBetData, MongoSingleBetData>();
        builder.Services.AddTransient<IMongoParleyBetSlipData, MongoParleyBetSlipData>();
        builder.Services.AddTransient<IMongoHouseAccountData, MongoHouseAccountData>();
    }
}


