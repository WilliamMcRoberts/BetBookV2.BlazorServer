
namespace BetBookGamingData.Services;

public class UpdateSingleBetsTimer : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromHours(3));
    private readonly ILogger<UpdateSingleBetsTimer> _logger;
    private readonly IGameService _gameService;
    private readonly IMongoSingleBetData _singleBetData;
    int week;
    SeasonType season;

    public UpdateSingleBetsTimer(
        ILogger<UpdateSingleBetsTimer> logger, IGameService gameService, IMongoSingleBetData singleBetData)
    {
        _logger = logger;
        _gameService = gameService;
        _singleBetData = singleBetData;
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
                await _singleBetData.UpdateFinishedSingleBets(week, season, _gameService);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed To Update Single Bets Timer / SingleBetsTimer");
            }
        }
    }
}
