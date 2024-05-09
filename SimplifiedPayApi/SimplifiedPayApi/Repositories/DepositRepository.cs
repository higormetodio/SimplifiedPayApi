using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class DepositRepository : Repository<Deposit>, IDepositRepository
{
    public DepositRepository(AppDbContext context) : base(context)
    {
    }

    public ICollection<Deposit>? GetDepositsByWallet(int id)
        => GetAll().Where(d => d.DepositorId == id).ToList();
}
