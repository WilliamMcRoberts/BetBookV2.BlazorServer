

namespace BetBookGamingData.Models;

public class HouseAccountModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string HouseId { get; set; }
    public decimal HouseAccountBalance { get; set; }
}
