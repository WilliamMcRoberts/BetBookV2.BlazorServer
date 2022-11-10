
using BetBookGamingData.Data;
using BetBookGamingData.Dto;
using BetBookGamingData.Helpers;
using BetBookGamingData.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BetBookGamingData.Services;

public class UpdateParleyBetsTimer : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(3));
    private readonly ILogger<UpdateParleyBetsTimer> _logger;
    private readonly IGameService _gameService;
    private readonly IMongoParleyBetSlipData _parleyBetData;
    int week;
    SeasonType season;

    public UpdateParleyBetsTimer(
        ILogger<UpdateParleyBetsTimer> logger, IGameService gameService, IMongoParleyBetSlipData parleyBetData)
    {
        _logger = logger;
        _gameService = gameService;
        _parleyBetData = parleyBetData;
    }

    protected async override Task ExecuteAsync(CancellationToken _stoppingToken)
    {

        while (await _timer.WaitForNextTickAsync(_stoppingToken)
                    && !_stoppingToken.IsCancellationRequested)
        {
            season = DateTime.Now.CalculateSeason();
            week = season.CalculateWeek(DateTime.Now);

            try
            {
                await _parleyBetData.UpdateFinishedParleyBets(week, season, _gameService);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed To Update Parley Bets / Parley Bets Timer");
            }
        }
    }
}
