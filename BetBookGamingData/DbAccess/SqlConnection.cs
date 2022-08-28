using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BetBookGamingData.DbAccess;

#nullable enable

public class SqlConnection : ISqlConnection
{
    private readonly IConfiguration _config;


    public SqlConnection(IConfiguration config)
    {
        _config = config;
    }

    
    public async Task<IEnumerable<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string betBookGamingSqlDatabase)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(_config.GetSection(
                "ConnectionStrings").GetSection(nameof(betBookGamingSqlDatabase)).Value);

        return await connection.QueryAsync<T>(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    
    public async Task SaveData<T>(
        string storedProcedure, T parameters, string betBookGamingSqlDatabase)
    {
        using IDbConnection connection =
            new System.Data.SqlClient.SqlConnection(_config.GetSection(
                "ConnectionStrings").GetSection(nameof(betBookGamingSqlDatabase)).Value);

        await connection.ExecuteAsync(
            storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }
}

#nullable restore
