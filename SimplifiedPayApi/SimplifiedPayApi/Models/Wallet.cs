using System.Text.Json.Serialization;

namespace SimplifiedPayApi.Models;

public class Wallet
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public decimal Balance { get; set; }
    public UserType UserType { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public ICollection<Transaction>? Transactions { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public ICollection<Deposit>? Deposits { get; set; }
}
