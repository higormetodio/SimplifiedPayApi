namespace SimplifiedPayApi.Models;

public class Deposit
{
    public int Id { get; set; }
    public int DepositorId { get; set; }
    public Wallet? Depositor { get; set; }
}
