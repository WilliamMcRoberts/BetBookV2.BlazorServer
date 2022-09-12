

using BetBookGamingData.Data;
using BetBookGamingData.Dto;
using BetBookGamingData.Services;

namespace BetBookGamingData.Helpers;

public static class UpdateHelpers
{
    public static async Task UpdateFinishedSingleBets(this IMongoSingleBetData singleBetData, int week, SeasonType season,  IGameService gameService)
    {
        GameDto[] games = await gameService.GetGamesByWeek(season, week);

        List<GameDto> finishedGames = games.Where(g => g.IsOver).ToList();

        foreach (GameDto game in finishedGames)
        {
            bool getHomeScore = decimal.TryParse(game.HomeScore?.ToString(), out var homeScore);
            bool getAwayScore = decimal.TryParse(game.AwayScore?.ToString(), out var awayScore);

            decimal totalScore = homeScore + awayScore;

            List<SingleBetModel> singleBetsOnGameInProgress =
                await singleBetData.GetAllSingleBetsOnGameInProgress(game.ScoreID);

            foreach (SingleBetModel singleBet in singleBetsOnGameInProgress)
            {
                if (singleBet.BetType == BetType.MONEYLINE)
                {
                    string winner = homeScore > awayScore ? game.HomeTeam
                        : homeScore < awayScore ? game.AwayTeam
                        : string.Empty;

                    singleBet.SingleBetStatus = winner == singleBet.WinnerChosen ? SingleBetStatus.WINNER
                        : winner == string.Empty ? SingleBetStatus.PUSH : SingleBetStatus.LOSER;

                    await singleBetData.UpdateSingleBet(singleBet);
                }

                else if (singleBet.BetType == BetType.POINTSPREAD)
                {
                    string winningTeam = string.Empty;

                    if (singleBet.WinnerChosen == game.HomeTeam)
                    {
                        decimal finalHomeScore = singleBet.PointsAfterSpread + homeScore;

                        winningTeam = finalHomeScore > awayScore ? game.HomeTeam
                            : finalHomeScore < awayScore ? game.AwayTeam
                            : string.Empty;
                    }

                    else if (singleBet.WinnerChosen == game.AwayTeam)
                    {
                        decimal finalAwayScore = singleBet.PointsAfterSpread + awayScore;

                        winningTeam = homeScore > finalAwayScore ? game.HomeTeam
                            : homeScore < finalAwayScore ? game.AwayTeam
                            : string.Empty;
                    }

                    singleBet.SingleBetStatus =
                        singleBet.WinnerChosen == winningTeam ? SingleBetStatus.WINNER
                        : winningTeam == string.Empty ? SingleBetStatus.PUSH
                        : SingleBetStatus.LOSER;

                    await singleBetData.UpdateSingleBet(singleBet);
                }

                else if (singleBet.BetType == BetType.OVERUNDER)
                {
                    if (singleBet.WinnerChosen[0] == 'O')
                    {
                        singleBet.SingleBetStatus =
                            totalScore > singleBet.OverUnder ? SingleBetStatus.WINNER
                            : totalScore < singleBet.OverUnder ? SingleBetStatus.LOSER
                            : SingleBetStatus.PUSH;

                        await singleBetData.UpdateSingleBet(singleBet);
                    }

                    singleBet.SingleBetStatus =
                            totalScore < singleBet.OverUnder ? SingleBetStatus.WINNER
                            : totalScore > singleBet.OverUnder ? SingleBetStatus.LOSER
                            : SingleBetStatus.PUSH;

                    await singleBetData.UpdateSingleBet(singleBet);
                }
            }
        }
    }

    public static async Task UpdateFinishedParleyBets(this IMongoParleyBetSlipData parleyBetData, int week, SeasonType season, IGameService gameService)
    {
        List<ParleyBetSlipModel> parleyBetSlipsInProgress = await parleyBetData.GetAllParleyBetSlipsInProgress();

        GameDto[] games = await gameService.GetGamesByWeek(season, week);

        List<GameDto> finishedGames = games.Where(g => g.IsOver).ToList();

        foreach (ParleyBetSlipModel parleyBet in parleyBetSlipsInProgress)
        {
            foreach (SingleBetForParleyModel singleBetForParley in parleyBet.SingleBetsForParleyList)
            {
                GameDto game =
                    games.Where(g => g.ScoreID == singleBetForParley.ScoreIdOfGame)
                         .FirstOrDefault();

                if (game.IsOver)
                {
                    singleBetForParley.SingleBetForParleyStatus =
                        singleBetForParley.ProcessFinishedSingleBetForParley(game);
                }
            }

            if (parleyBet.SingleBetsForParleyList.Count == parleyBet.SingleBetsForParleyList.Where(b => b.SingleBetForParleyStatus != SingleBetForParleyStatus.IN_PROGRESS).Count())
            {
                if (parleyBet.CheckIfParleyBetLoser())
                    parleyBet.ParleyBetSlipStatus = ParleyBetSlipStatus.LOSER;

                if (parleyBet.CheckIfParleyBetWinner())
                    parleyBet.ParleyBetSlipStatus = ParleyBetSlipStatus.WINNER;

                if (parleyBet.CheckIfParleyBetPush())
                    parleyBet.ParleyBetSlipStatus = ParleyBetSlipStatus.PUSH;
            }

            await parleyBetData.UpdateParleyBetSlip(parleyBet);
        }
    }

    public static bool CheckIfParleyBetLoser(this ParleyBetSlipModel parleyBet)
    {
        foreach (SingleBetForParleyModel bet in parleyBet.SingleBetsForParleyList)
        {
            if (bet.SingleBetForParleyStatus == SingleBetForParleyStatus.LOSER)
                return true;
        }

        return false;
    }

    public static bool CheckIfParleyBetPush(this ParleyBetSlipModel parleyBet)
    {
        foreach (SingleBetForParleyModel bet in parleyBet.SingleBetsForParleyList)
        {
            if (bet.SingleBetForParleyStatus != SingleBetForParleyStatus.PUSH)
                return false;
        }

        return true;
    }

    public static bool CheckIfParleyBetWinner(this ParleyBetSlipModel parleyBet)
    {
        foreach (SingleBetForParleyModel bet in parleyBet.SingleBetsForParleyList)
        {
            if (bet.SingleBetForParleyStatus != SingleBetForParleyStatus.WINNER)
                return false;
        }

        return true;
    }

    public static SingleBetForParleyStatus ProcessFinishedSingleBetForParley(
        this SingleBetForParleyModel singleBetForParley, GameDto game)
    {
        bool getHomeScore = decimal.TryParse(game.HomeScore?.ToString(), out var homeScore);
        bool getAwayScore = decimal.TryParse(game.AwayScore?.ToString(), out var awayScore);

        decimal totalScore = homeScore + awayScore;

        if (singleBetForParley.BetType == BetType.MONEYLINE)
        {
            string winner = homeScore > awayScore ? game.HomeTeam
                : homeScore < awayScore ? game.AwayTeam
                : string.Empty;

            return winner == singleBetForParley.WinnerChosen ? SingleBetForParleyStatus.WINNER
                : winner == string.Empty ? SingleBetForParleyStatus.PUSH : SingleBetForParleyStatus.LOSER;

        }

        else if (singleBetForParley.BetType == BetType.POINTSPREAD)
        {
            string winningTeam = string.Empty;

            if (singleBetForParley.WinnerChosen == game.HomeTeam)
            {
                decimal finalHomeScore = singleBetForParley.PointsAfterSpread + homeScore;

                winningTeam = finalHomeScore > awayScore ? game.HomeTeam
                    : finalHomeScore < awayScore ? game.AwayTeam
                    : string.Empty;
            }

            else if (singleBetForParley.WinnerChosen == game.AwayTeam)
            {
                decimal finalAwayScore = singleBetForParley.PointsAfterSpread + awayScore;

                winningTeam = homeScore > finalAwayScore ? game.HomeTeam
                    : homeScore < finalAwayScore ? game.AwayTeam
                    : string.Empty;
            }

            return
                singleBetForParley.WinnerChosen == winningTeam ? SingleBetForParleyStatus.WINNER
                : winningTeam == string.Empty ? SingleBetForParleyStatus.PUSH
                : SingleBetForParleyStatus.LOSER;

        }

        else if (singleBetForParley.BetType == BetType.OVERUNDER)
        {
            if (singleBetForParley.WinnerChosen[0] == 'O')
            {
                return
                    totalScore > singleBetForParley.OverUnder ? SingleBetForParleyStatus.WINNER
                    : totalScore < singleBetForParley.OverUnder ? SingleBetForParleyStatus.LOSER
                    : SingleBetForParleyStatus.PUSH;

            }

            return
                    totalScore < singleBetForParley.OverUnder ? SingleBetForParleyStatus.WINNER
                    : totalScore > singleBetForParley.OverUnder ? SingleBetForParleyStatus.LOSER
                    : SingleBetForParleyStatus.PUSH;

        }

        return SingleBetForParleyStatus.IN_PROGRESS;
    }
}
