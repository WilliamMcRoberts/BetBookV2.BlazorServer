using BetBookGamingData.Dto;

namespace BetBookGamingData.Interfaces;

public interface ITeamService
{
    Task<Team[]> GetAllTeams();
}
