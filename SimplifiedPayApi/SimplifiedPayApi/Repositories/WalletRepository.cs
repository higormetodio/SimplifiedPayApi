using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Pagination;

namespace SimplifiedPayApi.Repositories;

public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    public WalletRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Wallet> GetWallets(WalletsParameters walletsParameters)
    {
        var wallets = GetAll().OrderBy(w => w.FullName).AsQueryable();

        var orderWallets = PagedList<Wallet>.ToPagedList(wallets, walletsParameters.PageNumber, walletsParameters.PageSize);

        return orderWallets;
    }

    public PagedList<Wallet> GetWalletFullNameFilter(WalletFullNameFilter walletFullNameFilter)
    {
        var wallets = GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(walletFullNameFilter.FullName))
        {
            wallets = wallets.Where(w => w.FullName.Contains(walletFullNameFilter.FullName));
        }

        var filteredWallets = PagedList<Wallet>.ToPagedList(wallets, walletFullNameFilter.PageNumber, walletFullNameFilter.PageSize);

        return filteredWallets;
    }

    public PagedList<Wallet> GetWalletBalanceFilter(WalletBalanceFilter walletBalanceFilter)
    {
        var wallets = GetAll().AsQueryable();

        if (walletBalanceFilter.Balance.HasValue && !string.IsNullOrEmpty(walletBalanceFilter.BalanceStandard))
        {
            if (walletBalanceFilter.BalanceStandard.Equals("greater", StringComparison.OrdinalIgnoreCase))
            {
                wallets = wallets.Where(w => w.Balance > walletBalanceFilter.Balance.Value).OrderBy(w => w.Balance);
            }
            else if (walletBalanceFilter.BalanceStandard.Equals("less", StringComparison.OrdinalIgnoreCase))
            {
                wallets = wallets.Where(w => w.Balance < walletBalanceFilter.Balance.Value).OrderBy(w => w.Balance);
            }
            else if (walletBalanceFilter.BalanceStandard.Equals("equal", StringComparison.OrdinalIgnoreCase))
            {
                wallets = wallets.Where(w => w.Balance == walletBalanceFilter.Balance.Value).OrderBy(w => w.Balance);
            }
        }

        var filteredWallets = PagedList<Wallet>.ToPagedList(wallets, walletBalanceFilter.PageNumber, walletBalanceFilter.PageSize);

        return filteredWallets;
    }
}
