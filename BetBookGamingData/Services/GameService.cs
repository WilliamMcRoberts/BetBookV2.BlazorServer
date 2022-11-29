
namespace BetBookGamingData.Services;

public interface IGameService
{
    Task<GameByScoreIdDto> GetGameByScoreId(int scoreId);
    Task<GameDto[]> GetGamesByWeek(SeasonType currentSeason, int week, bool getFromCache = true);
}

#nullable enable

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameService> _logger;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;
    private const string CacheKey = "GameData";


    public GameService(IConfiguration config,
                       IHttpClientFactory httpClientFactory,
                       ILogger<GameService> logger,
                       IMemoryCache cache)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _cache = cache;
    }


    public async Task<GameByScoreIdDto> GetGameByScoreId(int scoreId)
    {
        GameByScoreIdDto? game = new();

        try
        {
            _logger.LogInformation("Calling Get Game By Score Id / Http Get");
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByScoreIdDto>(
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO:Key7").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Get Game By ScoreId / Http Get GameService");
        }

        return game!;
    }

    public async Task<GameDto[]> GetGamesByWeek(SeasonType currentSeason, int week, bool getFromCache = true)
    {
        var games = new GameDto[16];

        if(getFromCache)
        {
            games = _cache.Get<GameDto[]>(CacheKey);
            if (games is not null) return games;
        }

        try
        {
            _logger.LogInformation("Calling Get Games By Week / Http Get");
            var client = _httpClientFactory.CreateClient("sportsdata");

            var results = await client.GetFromJsonAsync<GameDto[]>(
                    $"scores/json/ScoresByWeek/2022{currentSeason}/{week}?key={_config.GetSection("SportsDataIO:Key7").Value}");

            games = results?.ToArray();

            _cache.Set(CacheKey, games, TimeSpan.FromMinutes(15));
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Get Games By Week / Http Get GameService");
        }
        
        return games!;
    }
}

#nullable restore
