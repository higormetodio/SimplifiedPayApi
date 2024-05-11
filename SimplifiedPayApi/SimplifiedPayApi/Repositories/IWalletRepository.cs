using SimplifiedPayApi.Models;
using SimplifiedPayApi.Pagination;

namespace SimplifiedPayApi.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        PagedList<Wallet> GetWallets(WalletsParameters walletsParameters);
        PagedList<Wallet> GetWalletFullNameFilter(WalletFullNameFilter walletFullNameFilter);
        PagedList<Wallet> GetWalletBalanceFilter(WalletBalanceFilter walletBalanceFilter);
    }
}
