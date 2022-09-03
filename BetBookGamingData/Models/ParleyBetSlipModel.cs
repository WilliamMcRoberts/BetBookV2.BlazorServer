

namespace BetBookGamingData.Models;

public class ParleyBetSlipModel
{
    public int Id { get; set; }
    public Guid ParleyBetSlipGuid { get; set; }
    public HashSet<CreateBetModel> SingleBetsHashSet { get; set; }
    public decimal ParleyBetAmount { get; set; }
    public decimal ParleyBetPayout { get; set; }
    public ParleyBetSlipPayoutStatus ParleyBetSlipPayoutStatus { get; set; }
    public ParleyBetSlipStatus ParleyBetSlipStatus { get; set; }
}
