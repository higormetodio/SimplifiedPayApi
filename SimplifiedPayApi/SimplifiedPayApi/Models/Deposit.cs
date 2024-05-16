using System.Text.Json.Serialization;

namespace SimplifiedPayApi.Models;

public class Deposit
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int DepositorId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Wallet? Depositor { get; set; }
    public DateTime TimeSpan { get; set; }
}
