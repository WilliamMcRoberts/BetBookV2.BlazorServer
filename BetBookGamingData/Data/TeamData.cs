using BetBookGamingData.Models;
using BetBookGamingData.DbAccess;

namespace BetBookGamingData.Data;

public interface ITeamData
{
    Task DeleteTeam(int id);
    Task<TeamModel> GetTeam(int id);
    Task<IEnumerable<TeamModel>> GetTeams();
    Task<int> InsertTeam(TeamModel team);
    Task UpdateTeam(TeamModel team);
}

#nullable enable

public class TeamData : ITeamData
{
    private readonly ISqlConnection _db;


    public TeamData(ISqlConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TeamModel>> GetTeams() =>
        await _db.LoadData<TeamModel, dynamic>("dbo.spTeams_GetAll", new { });


    public async Task<TeamModel?> GetTeam(int id)
    {
        var results = await _db.LoadData<TeamModel, dynamic>(
            "dbo.spTeams_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }


    public async Task<int> InsertTeam(TeamModel team)
    {
        await _db.SaveData("dbo.spTeams_Insert", new
        {
            team.TeamName,
            team.City,
            team.Stadium,
            team.Wins,
            team.Losses,
            team.Draws,
            team.Symbol,
            team.Division,
            team.Conference
        });

        return team.Id;
    }

    public async Task UpdateTeam(TeamModel team)
    {
        await _db.SaveData("dbo.spTeams_Update", new
        {
            team.Id,
            team.TeamName,
            team.City,
            team.Stadium,
            team.Wins,
            team.Losses,
            team.Draws,
            team.Symbol,
            team.Division,
            team.Conference
        });
    }

    public async Task DeleteTeam(int id)
    {
        await _db.SaveData(
        "dbo.spTeams_Delete", new
        {
            Id = id
        });
    }
}

#nullable restore
