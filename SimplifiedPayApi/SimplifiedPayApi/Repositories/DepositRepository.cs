using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class DepositRepository : Repository<Deposit>, IDepositRepository
{
    public DepositRepository(AppDbContext context) : base(context)
    {
    }

    public Deposit? GetDepositByWallet(int id)
        =>  _context.Deposits.Include(d => d.Depositor)
                         .FirstOrDefault(d => d.DepositorId == id);
}
