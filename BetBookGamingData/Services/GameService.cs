
using BetBookGamingData.Dto;
using BetBookGamingData.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Services;

public interface IGameService
{
    Task<GameByScoreIdDto> GetGameByScoreId(int scoreId);
    Task<GameByTeamDto> GetGameByTeam(TeamModel team);
    Task<GameDto[]> GetGamesByWeek(SeasonType currentSeason, int week);
}

#nullable enable

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GameService> _logger;
    private readonly IConfiguration _config;
    IEnumerable<TeamModel>? teams;
    IEnumerable<GameModel>? games;


    public GameService(
                       IConfiguration config,
                       IHttpClientFactory httpClientFactory,
                       ILogger<GameService> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<GameByTeamDto> GetGameByTeam(TeamModel team)
    {
        GameByTeamDto? game = new();

        try
        {
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByTeamDto>(
                    $"odds/json/TeamTrends/{team.Symbol}?key={_config.GetSection("SportsDataIO").GetSection("Key4").Value}");

        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return game!;
    }

    public async Task<GameByScoreIdDto> GetGameByScoreId(int scoreId)
    {
        GameByScoreIdDto? game = new();

        try
        {
            var client = _httpClientFactory.CreateClient("sportsdata");

            game = await client.GetFromJsonAsync<GameByScoreIdDto>(
                    $"stats/json/BoxScoreByScoreIDV3/{scoreId}?key={_config.GetSection("SportsDataIO").GetSection("Key4").Value}");
        }

        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }

        return game!;
    }

    public async Task<GameDto[]> GetGamesByWeek(SeasonType currentSeason, int week)
    {

        GameDto[]? games = new GameDto[16];

        try
        {
            _logger.LogInformation("Calling Get Games By Week...");
            var client = _httpClientFactory.CreateClient("sportsdata");

            games = await client.GetFromJsonAsync<GameDto[]>(
                    $"scores/json/ScoresByWeek/2022{currentSeason}/{week}?key={_config.GetSection("SportsDataIO").GetSection("Key4").Value}");
        }

        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Exception On Get Games By Week");
        }

        return games!;
    }
}

#nullable restore
