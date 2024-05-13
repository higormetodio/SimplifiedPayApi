using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Pagination;

namespace SimplifiedPayApi.Repositories;

public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    public WalletRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Wallet>> GetWalletsAsync(WalletsParameters walletsParameters)
    {
        var wallets = await GetAllAsync();
            
        var orderWallets = wallets.OrderBy(w => w.FullName).AsQueryable();

        var results = PagedList<Wallet>.ToPagedList(orderWallets, walletsParameters.PageNumber, walletsParameters.PageSize);

        return results;
    }

    public async Task<PagedList<Wallet>> GetWalletFullNameFilterAsync(WalletFullNameFilter walletFullNameFilter)
    {
        var wallets = await GetAllAsync();
           
        var walletsQuery = wallets.AsQueryable();

        if (!string.IsNullOrEmpty(walletFullNameFilter.FullName))
        {
            walletsQuery = walletsQuery.Where(w => w.FullName.Contains(walletFullNameFilter.FullName));
        }

        var filteredWallets = PagedList<Wallet>.ToPagedList(walletsQuery, walletFullNameFilter.PageNumber, walletFullNameFilter.PageSize);

        return filteredWallets;
    }

    public async Task<PagedList<Wallet>> GetWalletBalanceFilterAsync(WalletBalanceFilter walletBalanceFilter)
    {
        var wallets = await GetAllAsync();
        
        var walletsQuery = wallets.AsQueryable();

        if (walletBalanceFilter.Balance.HasValue && !string.IsNullOrEmpty(walletBalanceFilter.BalanceStandard))
        {
            if (walletBalanceFilter.BalanceStandard.Equals("greater", StringComparison.OrdinalIgnoreCase))
            {
                walletsQuery = walletsQuery.Where(w => w.Balance > walletBalanceFilter.Balance.Value).OrderBy(w => w.Balance);
            }
            else if (walletBalanceFilter.BalanceStandard.Equals("less", StringComparison.OrdinalIgnoreCase))
            {
                walletsQuery = walletsQuery.Where(w => w.Balance < walletBalanceFilter.Balance.Value).OrderBy(w => w.Balance);
            }
            else if (walletBalanceFilter.BalanceStandard.Equals("equal", StringComparison.OrdinalIgnoreCase))
            {
                walletsQuery = walletsQuery.Where(w => w.Balance == walletBalanceFilter.Balance.Value).OrderBy(w => w.Balance);
            }
        }

        var filteredWallets = PagedList<Wallet>.ToPagedList(walletsQuery, walletBalanceFilter.PageNumber, walletBalanceFilter.PageSize);

        return filteredWallets;
    }
}
