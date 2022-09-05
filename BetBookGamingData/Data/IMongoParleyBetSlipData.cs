namespace BetBookGamingData.Data
{
    public interface IMongoParleyBetSlipData
    {
        Task CreateParleyBetSlip(ParleyBetSlipModel parleyBetSlip, UserModel user);
    }
}