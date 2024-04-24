namespace SimplifiedPayApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public User? Payer { get; set; }
    public User? Receiver { get; set; }
    public bool Status { get; set; }
    public DateTime Timestamp { get; set; }
}
