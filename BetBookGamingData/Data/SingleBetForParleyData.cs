

using BetBookGamingData.DbAccess;
using Microsoft.Extensions.Configuration;

namespace BetBookGamingData.Data;

public class SingleBetForParleyData
{
    private readonly ISqlConnection _database;
    private readonly IConfiguration _configuration;

    public SingleBetForParleyData(ISqlConnection database, IConfiguration configuration)
	{
        _database = database;
        _configuration = configuration;
    }
}
