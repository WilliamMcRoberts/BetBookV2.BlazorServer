
using NetEscapades.EnumGenerators;

namespace BetBookGamingData;

[EnumExtensions]
public enum SeasonType
{
    PRE = 0,
    REG = 1,
    POST = 2
}

[EnumExtensions]
public enum GameStatus
{
    NOT_STARTED = 0,
    IN_PROGRESS = 1,
    FINISHED = 2
}

[EnumExtensions]
public enum ParleyPayoutStatus
{
    UNPAID,
    PAID
}

[EnumExtensions]
public enum OverUnder
{
    OVER,
    UNDER
}

[EnumExtensions]
public enum BetType
{
    POINTSPREAD,
    MONEYLINE,
    OVERUNDER
}

[EnumExtensions]
public enum ParleyBetSlipPayoutStatus
{
    UNPAID,
    PAID
}


[EnumExtensions]
public enum ParleyBetSlipStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

[EnumExtensions]
public enum SingleBetStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}

[EnumExtensions]
public enum SingleBetPayoutStatus
{
    UNPAID,
    PAID
}

[EnumExtensions]
public enum SingleBetForParleyStatus
{
    IN_PROGRESS,
    WINNER,
    LOSER,
    PUSH
}


