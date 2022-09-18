using BetBookGamingData.Data;
using BetBookGamingData.Dto;
using BetBookGamingData.Helpers;

namespace BetBookGamingData.Services;

public class BetSlipState
{
    public List<CreateBetModel> preBets = new();
    public UserModel loggedInUser;
    public bool conflictingBetsForParley;
    public decimal totalWagerForParley;
    public decimal totalPayoutForParley;
    public bool parleyBetAmountBad;
    public bool gameHasStarted;
    public bool betAmountForSinglesBad;
    public bool betAmountForParleyBad;
    public SeasonType season;
    public int week;
    public string startedGameDescription;
    public readonly IGameService _gameService;
    public readonly IMongoParleyBetSlipData _parleyBetSlipData;
    public readonly IMongoSingleBetData _singleBetData;

    public BetSlipState(IGameService gameService, 
                        IMongoParleyBetSlipData parleyBetSlipData, 
                        IMongoSingleBetData singleBetData)
    {
        _gameService = gameService;
        _parleyBetSlipData = parleyBetSlipData;
        _singleBetData = singleBetData;
    }

    public void SelectOrRemoveWinnerAndGameForBet(string winner, GameDto game, BetType betType)
    {
        if (preBets.Contains(
            preBets.Where(b => b.Winner == winner && b.Game == game && b.BetType == betType)
                   .FirstOrDefault()!))
        {
            preBets.Remove(
            preBets.Where(b => b.Winner == winner && b.Game == game && b.BetType == betType)
                   .FirstOrDefault()!);

            conflictingBetsForParley = CheckForConflictingBets();

            return;
        }

        preBets.Add(new CreateBetModel
        {
            Id = Guid.NewGuid(),
            BetType = betType,
            BetAmount = 0,
            MoneylinePayout = GetMoneylinePayoutForBet(winner, game, betType),
            Game = game,
            Winner = winner,
            PointSpread = Math.Round(Convert.ToDecimal(game.PointSpread), 1),
            OverUnder = Math.Round(Convert.ToDecimal(game.OverUnder), 1),
        });

        conflictingBetsForParley = CheckForConflictingBets();
    }

    public bool CheckForConflictingBets()
    {
        foreach (CreateBetModel cb in preBets)
        {
            if (preBets.Where(b => b.Game == cb.Game && b.BetType == cb.BetType).Count() > 1)
                return true;
        }

        return false;
    }

    public async Task<bool> OnSubmitBetsFromSinglesBetSlip(UserModel loggedInUser)
    {
        if(preBets.Count < 1) return false;

        season = DateTime.Now.CalculateSeason();
        week = season.CalculateWeek(DateTime.Now);

        GameDto[] gameCheckArray =
            await _gameService.GetGamesByWeek(season, week);

        foreach (CreateBetModel createBetModel in preBets)
        {
            if (createBetModel.BetAmount <= 0)
            {
                betAmountForSinglesBad = true;
                return false;
            }

            GameDto game = gameCheckArray.Where(g => g.ScoreID == createBetModel.Game.ScoreID)
                .FirstOrDefault()!;

            if (game.HasStarted)
            {
                gameHasStarted = true;
                startedGameDescription = $"{game.AwayTeam} at {game.HomeTeam}";
                return false;
            }

            var singleBet = new SingleBetModel
            {
                WinnerChosen = createBetModel.BetType == BetType.OVERUNDER ?
                               (createBetModel.Winner[0] == 'O' ? "Over" : "Under")
                                : createBetModel.Winner,

                BetPayout = 
                    Math.Round(CalculateSingleBetPayout(
                        createBetModel.BetAmount, createBetModel.MoneylinePayout), 2),

                BettorId = loggedInUser.UserId,
                BetType = createBetModel.BetType,
                BetAmount = createBetModel.BetAmount,
                SingleBetStatus = SingleBetStatus.IN_PROGRESS,
                SingleBetPayoutStatus = SingleBetPayoutStatus.UNPAID,
                GameSnapshot = CreateGameSnapshot(createBetModel.Game),
                PointsAfterSpread = CalculatePointsAfterSpread(
                            createBetModel.Game, createBetModel.Winner)
            };

            await _singleBetData.CreateSingleBet(singleBet);
        }

        preBets.Clear();
        return true;
    }

    public async Task<bool> OnSubmitBetsFromParleyBetSlip(UserModel loggedInUser)
    {
        if (preBets.Count < 1) 
            return false;

        if (conflictingBetsForParley)
            return false;

        if (totalWagerForParley <= 0)
        {
            betAmountForParleyBad = true;
            return false;
        }

        season = DateTime.Now.CalculateSeason();
        week = season.CalculateWeek(DateTime.Now);

        GameDto[] gameCheckArray =
            await _gameService.GetGamesByWeek(season, week);

        var parleyBetSlip = new ParleyBetSlipModel();

        foreach (CreateBetModel createBetModel in preBets)
        {
            GameDto game = gameCheckArray.Where(g => g.ScoreID == createBetModel.Game.ScoreID)
                .FirstOrDefault()!;

            if (game.HasStarted)
            {
                gameHasStarted = true;
                return false;
            }

            parleyBetSlip.SingleBetsForParleyList.Add(new SingleBetForParleyModel
            {
                WinnerChosen = createBetModel.BetType == BetType.OVERUNDER ?
                               (createBetModel.Winner[0] == 'O' ? "Over" : "Under")
                                : createBetModel.Winner,

                PointsAfterSpread =
                    CalculatePointsAfterSpread(createBetModel.Game, createBetModel.Winner),

                BettorId = loggedInUser.UserId,
                BetType = createBetModel.BetType,
                SingleBetForParleyStatus = SingleBetForParleyStatus.IN_PROGRESS,
                GameSnapshot = CreateGameSnapshot(createBetModel.Game)
            });

        }

        parleyBetSlip.BettorId = loggedInUser.UserId;
        parleyBetSlip.ParleyBetAmount = totalWagerForParley;
        parleyBetSlip.ParleyBetPayout = Math.Round(totalPayoutForParley, 2);
        parleyBetSlip.ParleyBetSlipStatus = ParleyBetSlipStatus.IN_PROGRESS;
        parleyBetSlip.ParleyBetSlipPayoutStatus = ParleyBetSlipPayoutStatus.UNPAID;

        bool parleyBetGood =
            await _parleyBetSlipData.CreateParleyBetSlip(parleyBetSlip);

        preBets.Clear();
        totalWagerForParley = 0;
        totalPayoutForParley = 0;

        return parleyBetGood;
    }

    public GameSnapshotModel CreateGameSnapshot(GameDto gameDto) =>
        new GameSnapshotModel
        {
            Week = gameDto.Week,
            Date = gameDto.Date,
            AwayTeam = gameDto.AwayTeam,
            HomeTeam = gameDto.HomeTeam,
            PointSpread = Math.Round(Convert.ToDecimal(gameDto.PointSpread), 1),
            OverUnder = Math.Round(Convert.ToDecimal(gameDto.OverUnder), 1),
            AwayTeamMoneyLine = gameDto.AwayTeamMoneyLine,
            HomeTeamMoneyLine = gameDto.HomeTeamMoneyLine,
            PointSpreadAwayTeamMoneyLine = gameDto.PointSpreadAwayTeamMoneyLine,
            PointSpreadHomeTeamMoneyLine = gameDto.PointSpreadHomeTeamMoneyLine,
            ScoreID = gameDto.ScoreID,
            OverPayout = gameDto.OverPayout,
            UnderPayout = gameDto.UnderPayout
        };

    public decimal CalculatePointsAfterSpread(GameDto game, string chosenWinner) =>
         chosenWinner == game.HomeTeam ? 0 + (decimal)game.PointSpread! 
            : 0 - (decimal)game.PointSpread!;

    public decimal CalculateSingleBetPayout(decimal betAmount, int moneylinePayout) =>
         moneylinePayout < 0 ? betAmount / ((decimal)moneylinePayout * -1 / 100) + betAmount
         : ((decimal)moneylinePayout / 100) * betAmount;

    public decimal GetPayoutForTotalBetsParley()
    {
        if (preBets.Count < 2) return 0;

        decimal totalDecimalOdds = 1;

        foreach (CreateBetModel createBetModel in preBets)
        {
            decimal decimalMoneyline =
                ConvertMoneylinePayoutToDecimalFormat(createBetModel.MoneylinePayout);

            totalDecimalOdds *= decimalMoneyline;
        }

        return totalPayoutForParley = totalWagerForParley * totalDecimalOdds;
    }

    public decimal ConvertMoneylinePayoutToDecimalFormat(int moneylinePayout) =>
         moneylinePayout < 0 ? (100 / (decimal)moneylinePayout * -1) + (decimal)1
         : ((decimal)moneylinePayout / 100) + 1;

    public void RemoveBetFromPreBetsList(CreateBetModel createBetModel) =>
        preBets.Remove(createBetModel);

    public decimal GetPayoutForTotalBetsSingles()
    {
        decimal total = 0;

        foreach (CreateBetModel createBetModel in preBets)
        {
            decimal betPayout = createBetModel.MoneylinePayout < 0 ?
                     createBetModel.BetAmount / ((decimal)createBetModel.MoneylinePayout * -1 / 100) + createBetModel.BetAmount
                     : ((decimal)createBetModel.MoneylinePayout / 100) * createBetModel.BetAmount;

            total += betPayout;
        }

        return total;
    }

    public int GetMoneylinePayoutForBet(string winner, GameDto game, BetType betType) =>
        betType == BetType.POINTSPREAD ? 
        (winner == game.AwayTeam ? (int)game.PointSpreadAwayTeamMoneyLine! : (int)game.PointSpreadHomeTeamMoneyLine!) 
        : betType == BetType.OVERUNDER ? (winner[0] == 'O' ? (int)game.OverPayout! : (int)game.UnderPayout!) 
        : (winner == game.AwayTeam ? (int)game.AwayTeamMoneyLine! : (int)game.HomeTeamMoneyLine!);

    public string GetWinnerSummary(CreateBetModel createBetModel) => 
        createBetModel.BetType == BetType.POINTSPREAD ? GetWinnerSummaryForPointSpread(createBetModel)
        : createBetModel.BetType == BetType.OVERUNDER ? GetWinnerSummaryForOverUnder(createBetModel)
        : createBetModel.Winner;

    public string GetWinnerSummaryForOverUnder(CreateBetModel createBetModel) =>
         createBetModel.Winner[0] == 'O' ? $"Over {createBetModel.Game.OverUnder}"
         : $"Under {createBetModel.Game.OverUnder}";

    public string GetWinnerSummaryForPointSpread(CreateBetModel createBetModel) =>
         createBetModel.Winner == createBetModel.Game.HomeTeam ?
         $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):-#.#;+#.#;+0}"
         : $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread * -1):-#.#;+#.#;+0}";
}
