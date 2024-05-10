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
}
