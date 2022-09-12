namespace BetBookGamingData.Data
{
    public interface IMongoParleyBetSlipData
    {
        Task CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip, UserModel user);
        Task<List<ParleyBetSlipModel>> GetAllParleyBetSlipsInProgress();
        Task<List<ParleyBetSlipModel>> GetBettorParleyBetSlips(string userId);
        Task UpdateParleyBetSlip(ParleyBetSlipModel parleyBetSlip);
    }
}