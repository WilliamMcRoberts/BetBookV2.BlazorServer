namespace BetBookGamingData.Data;

public interface IMongoParleyBetSlipData
{
    Task<bool> CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip);
    Task<List<ParleyBetSlipModel>> GetAllParleyBetSlipsInProgress();
    Task<List<ParleyBetSlipModel>> GetBettorParleyBetSlips(string userId);
    Task UpdateParleyBetSlip(ParleyBetSlipModel parleyBetSlip);
}