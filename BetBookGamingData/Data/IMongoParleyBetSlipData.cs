namespace BetBookGamingData.Data
{
    public interface IMongoParleyBetSlipData
    {
        Task CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip, UserModel user);
        Task<List<ParleyBetSlipModel>> GetBettorParleyBetSlips(string userId);
    }
}