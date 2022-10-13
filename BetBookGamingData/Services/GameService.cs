
using BetBookGamingData.Dto;
using BetBookGamingData.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using BetBookGamingData.Data;

namespace BetBookGamingData.Services;

public interface IGameService
{
    Task<GameByScoreIdDto> GetGameByScoreId(int scoreId);
    Task<GameDto[]> GetGamesByWeek(SeasonType currentSeason, int week);
}

#nullable enable

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameService> _logger;
    private readonly IConfiguration _config;

    public GameService(
                       IConfiguration config,
                       IHttpClientFactory httpClientFactory,
                       ILogger<GameService> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }


    public async Task<GameByScoreIdDto> GetGameByScoreId(int scoreId)
    {
        GameByScoreIdDto? game = new();

        try
        {
            _logger.LogInformation("Calling Get Game By Score Id / Http Get");
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByScoreIdDto>(
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO:Key6").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Get Game By ScoreId / Http Get GameService");
        }

        return game!;
    }

    public async Task<GameDto[]> GetGamesByWeek(SeasonType currentSeason, int week)
    {
        GameDto[]? games = new GameDto[16];

        try
        {
            _logger.LogInformation("Calling Get Games By Week / Http Get");
            var client = _httpClientFactory.CreateClient("sportsdata");

            games = await client.GetFromJsonAsync<GameDto[]>(
                    $"scores/json/ScoresByWeek/2022{currentSeason}/{week}?key={_config.GetSection("SportsDataIO:Key6").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Failed To Get Games By Week / Http Get GameService");
        }

        return games!;
    }
}

#nullable restore
