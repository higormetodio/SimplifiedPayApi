using System.Text.Json.Serialization;

namespace SimplifiedPayApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public User? Payer { get; set; }
    public int PayerId { get; set; }
    public User? Receiver { get; set; }
    public int ReceiverId { get; set; }
    public bool Status { get; set; }
    public DateTime Timestamp { get; set; }
}
