

namespace BetBookGamingData.Helpers;

public static class QueryHelpers
{
    public static string GetWinnerSummary(this CreateBetModel createBetModel) =>
        createBetModel.BetType == BetType.POINTSPREAD ? GetWinnerSummaryForPointSpread(createBetModel)
        : createBetModel.BetType == BetType.OVERUNDER ? GetWinnerSummaryForOverUnder(createBetModel)
        : createBetModel.Winner;

    public static string GetWinnerSummaryForOverUnder(CreateBetModel createBetModel) =>
         createBetModel.Winner[0] == 'O' ? $"Over {createBetModel.Game.OverUnder}"
         : $"Under {createBetModel.Game.OverUnder}";

    public static string GetWinnerSummaryForPointSpread(CreateBetModel createBetModel) =>
         createBetModel.Winner == createBetModel.Game.HomeTeam ?
         $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):+#.0;-#.0}"
         : $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):-#.0;+#.0;}";
}
