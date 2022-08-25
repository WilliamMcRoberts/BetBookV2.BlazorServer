
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
public enum BetStatus
{
    IN_PROGRESS = 0, 
    WINNER = 1,
    LOSER = 2,
    PUSH = 3
}

[EnumExtensions]
public enum PayoutStatus
{
    UNPAID = 0,
    PAID = 1, 
    PARLEY = 2
}

[EnumExtensions]
public enum ParleyBetStatus
{
    IN_PROGRESS = 0,
    WINNER = 1, 
    LOSER = 2,
    PUSH = 3
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
