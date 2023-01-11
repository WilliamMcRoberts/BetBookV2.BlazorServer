
namespace BetBookGamingData.Helpers;

#nullable enable

public static class CalculationHelpers
{

    public static int CalculateWeek(this SeasonType season, DateTime dateTime)
    {
        int week = season == SeasonType.PRE ? (dateTime - new DateTime(2022, 8, 9)).Days / 7
                   : season == SeasonType.REG ? (dateTime - new DateTime(2022, 9, 6)).Days / 7
                   : (dateTime - new DateTime(2023, 1, 14)).Days / 7;

        if (week < 0)
            return 0;

        return week + 1;
    }

    public static SeasonType CalculateSeason(this DateTime dateTime)
    {
        return dateTime > new DateTime(2022, 8, 9) && dateTime < new DateTime(2022, 8, 28) ? SeasonType.PRE
            : dateTime > new DateTime(2022, 8, 31) && dateTime < new DateTime(2023, 1, 10) ? SeasonType.REG
            : SeasonType.POST;
    }

    public static int GetMoneylinePayoutForBet(this string winner, GameDto game, BetType betType) =>
        betType == BetType.POINTSPREAD ?
        (winner == game.AwayTeam ? (int)game.PointSpreadAwayTeamMoneyLine! : (int)game.PointSpreadHomeTeamMoneyLine!)
        : betType == BetType.OVERUNDER ? (winner[0] == 'O' ? (int)game.OverPayout! : (int)game.UnderPayout!)
        : (winner == game.AwayTeam ? (int)game.AwayTeamMoneyLine! : (int)game.HomeTeamMoneyLine!);

    public static decimal CalculateSingleBetPayout(this decimal betAmount, int moneylinePayout) =>
         moneylinePayout < 0 ? betAmount / ((decimal)moneylinePayout * -1 / 100) + betAmount
         : ((decimal)moneylinePayout / 100) * betAmount;

    public static decimal CalculatePointsAfterSpread(this GameDto game, string chosenWinner) =>
         chosenWinner == game.HomeTeam ? 0 + (decimal)game.PointSpread!
            : 0 - (decimal)game.PointSpread!;

    public static decimal GetPayoutForTotalBetsParley(this List<CreateBetModel> betList, decimal totalWagerForParley)
    {
        if (betList.Count < 2) return 0;

        decimal totalDecimalOdds = 1;

        foreach (CreateBetModel createBetModel in betList)
        {
            decimal decimalMoneyline =
                ConvertMoneylinePayoutToDecimalFormat(createBetModel.MoneylinePayout);

            totalDecimalOdds *= decimalMoneyline;
        }

        return Math.Round(totalWagerForParley * totalDecimalOdds, 2);
    }

    public static decimal GetPayoutForTotalBetsSingles(this List<CreateBetModel> betList)
    {
        decimal total = 0;

        foreach (CreateBetModel createBetModel in betList)
        {
            decimal betPayout = createBetModel.MoneylinePayout < 0 ?
                     createBetModel.BetAmount / ((decimal)createBetModel.MoneylinePayout * -1 / 100) + createBetModel.BetAmount
                     : ((decimal)createBetModel.MoneylinePayout / 100) * createBetModel.BetAmount;

            total += betPayout;
        }

        return total;
    }

    public static decimal ConvertMoneylinePayoutToDecimalFormat(int moneylinePayout) =>
         moneylinePayout < 0 ? (100 / (decimal)moneylinePayout * -1) + (decimal)1
         : ((decimal)moneylinePayout / 100) + 1;
}

#nullable restore
