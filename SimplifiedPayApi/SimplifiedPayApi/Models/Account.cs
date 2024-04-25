namespace SimplifiedPayApi.Models;

public class Account
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public User? User { get; set; }
    public int UserId { get; set; }
    public DateTime Timestamp { get; set; }
}
