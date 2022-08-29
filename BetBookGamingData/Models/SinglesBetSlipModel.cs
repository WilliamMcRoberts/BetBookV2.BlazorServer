

namespace BetBookGamingData.Models;

public class SinglesBetSlipModel
{
    public int Id { get; set; }
    public HashSet<CreateBetModel> MyProperty { get; set; }
}
