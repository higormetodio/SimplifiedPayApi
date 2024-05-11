namespace SimplifiedPayApi.Pagination;

public class WalletBalanceFilter : QueryStringParameters
{
    public decimal? Balance { get; set; }
    public string? BalanceStandard { get; set; }
}
