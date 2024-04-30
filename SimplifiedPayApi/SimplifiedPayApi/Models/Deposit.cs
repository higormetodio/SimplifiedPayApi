using System.Text.Json.Serialization;

namespace SimplifiedPayApi.Models;

public class Deposit
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public User? User { get; set; }
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; }
}
