using SimplifiedPayApi.Models;
using SimplifiedPayApi.Pagination;

namespace SimplifiedPayApi.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<PagedList<Wallet>> GetWalletsAsync(WalletsParameters walletsParameters);
        Task<PagedList<Wallet>> GetWalletFullNameFilterAsync(WalletFullNameFilter walletFullNameFilter);
        Task<PagedList<Wallet>> GetWalletBalanceFilterAsync(WalletBalanceFilter walletBalanceFilter);
    }
}
