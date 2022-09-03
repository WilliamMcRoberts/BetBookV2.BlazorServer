

using BetBookGamingData.DbAccess;
using BetBookGamingData.Interfaces;
using BetBookGamingData.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BetBookGamingData.Data;

#nullable enable

public class SingleBetData : ISingleBetData
{
    private readonly ISqlConnection _database;
    private readonly IConfiguration _configuration;

    public SingleBetData(ISqlConnection database, IConfiguration configuration)
    {
        _database = database;
        _configuration = configuration;
    }

    public async Task<IEnumerable<SingleBetModel>> GetSingleBets()
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetSection("ConnectionStrings").GetSection("BetBookGamingSqlDatabase").Value);

        string sqlQuery = @"select * from dbo.SingleBets;";

        return await connection.QueryAsync<SingleBetModel>(sqlQuery);
    }

    public async Task<SingleBetModel?> GetSingleBetById(int singleBetId)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetSection("ConnectionStrings").GetSection("BetBookGamingSqlDatabase").Value);

        string sqlQuery = @$"select * from dbo.SingleBets where SingleBetId = {singleBetId};";

        return await connection.QueryFirstOrDefaultAsync<SingleBetModel>(sqlQuery);
    }

    public async Task<int> InsertSingleBet(SingleBetModel singleBet)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetSection("ConnectionStrings").GetSection("BetBookGamingSqlDatabase").Value);

        var parameters = new DynamicParameters();

        parameters.Add("@SingleBetId", 0, DbType.Int32, 
                ParameterDirection.Output);
        parameters.Add("@BetAmount", singleBet.BetAmount);
        parameters.Add("@BetPayout", singleBet.BetPayout);
        parameters.Add("@MoneylinePayout", singleBet.MoneylinePayout);
        parameters.Add("@BetType", singleBet.BetType.ToStringFast());
        parameters.Add("@SingleBetStatus", singleBet.SingleBetStatus.ToStringFast());
        parameters.Add("@SingleBetPayoutStatus", singleBet.SingleBetPayoutStatus.ToStringFast());
        parameters.Add("@ScoreIdOfGame", singleBet.Game?.ScoreID);
        parameters.Add("@WinnerChosen", singleBet.WinnerChosen);
        parameters.Add("@PointSpread", singleBet.PointSpread);
        parameters.Add("@OverUnder", singleBet.OverUnder);

        string sqlQuery =
                $@"insert into dbo.SingleBets 
                    (BetAmount, BetPayout, MoneylinePayout, BetType, SingleBetStatus, SingleBetPayoutStatus, ScoreIdOfGame, WinnerChosen, PointSpread, OverUnder)
                    output Inserted.SingleBetId
                    values (@BetAmount, @BetPayout, @MoneylinePayout, @BetType, @SingleBetStatus, @SingleBetPayoutStatus, @ScoreIdOfGame, @WinnerChosen, @PointSpread, @OverUnder)
                    select @SingleBetId = @@IDENTITY;";

        return await connection.QuerySingleAsync<int>(sqlQuery, parameters);
    }

    public async Task UpdateSingleBet(SingleBetModel singleBet)
    {
        using IDbConnection connection = new System.Data.SqlClient.SqlConnection(
            _configuration.GetSection("ConnectionStrings").GetSection("BetBookGamingSqlDatabase").Value);

        var parameters = new DynamicParameters();

        parameters.Add("@SingleBetId", singleBet.SingleBetId);
        parameters.Add("@BetAmount", singleBet.BetAmount);
        parameters.Add("@BetPayout", singleBet.BetPayout);
        parameters.Add("@MoneylinePayout", singleBet.MoneylinePayout);
        parameters.Add("@BetType", singleBet.BetType.ToStringFast());
        parameters.Add("@SingleBetStatus", singleBet.SingleBetStatus.ToStringFast());
        parameters.Add("@SingleBetPayoutStatus", singleBet.SingleBetPayoutStatus.ToStringFast());
        parameters.Add("@ScoreIdOfGame", singleBet.ScoreIdOfGame);
        parameters.Add("@WinnerChosen", singleBet.WinnerChosen);
        parameters.Add("@PointSpread", singleBet.PointSpread);
        parameters.Add("@OverUnder", singleBet.OverUnder);

        string sqlQuery =
                $@"update dbo.SingleBets 
                   set BetAmount = @BetAmount, BetPayout = @BetPayout, MoneylinePayout = @MoneylinePayout, BetType = @BetType, SingleBetStatus = @SingleBetStatus, SingleBetPayoutStatus = @SingleBetPayoutStatus, ScoreIdOfGame = @ScoreIdOfGame, WinnerChosen = @WinnerChosen, PointSpread = @PointSpread, OverUnder = @OverUnder
                   where SingleBetId = @SingleBetId;";

        await connection.ExecuteAsync(sqlQuery, parameters);
    }
}

#nullable disable