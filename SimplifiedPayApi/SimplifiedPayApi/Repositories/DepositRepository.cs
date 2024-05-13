using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class DepositRepository : Repository<Deposit>, IDepositRepository
{
    public DepositRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ICollection<Deposit>?> GetDepositsByWalletAsync(int id)
    {
        var deposits = await GetAllAsync();

        return deposits.Where(d => d.DepositorId == id).ToList();
    }
}
