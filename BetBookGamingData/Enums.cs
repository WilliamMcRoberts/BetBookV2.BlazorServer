
namespace BetBookGamingData;

public enum SeasonType
{
    PRE = 0,
    REG = 1,
    POST = 2
}

public enum GameStatus
{
    NOT_STARTED = 0,
    IN_PROGRESS = 1,
    FINISHED = 2
}

public enum BetStatus
{
    IN_PROGRESS = 0, 
    WINNER = 1,
    LOSER = 2,
    PUSH = 3
}

public enum PayoutStatus
{
    UNPAID = 0,
    PAID = 1, 
    PARLEY = 2
}

public enum ParleyBetStatus
{
    IN_PROGRESS = 0,
    WINNER = 1, 
    LOSER = 2,
    PUSH = 3
}

public enum ParleyPayoutStatus
{
    UNPAID,
    PAID
}

public enum OverUnder
{
    OVER,
    UNDER
}

public enum BetTypeNew
{
    POINTSPREAD,
    MONEYLINE,
    OVERUNDER
}
