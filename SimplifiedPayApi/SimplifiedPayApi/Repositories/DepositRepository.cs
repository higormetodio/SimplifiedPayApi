using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class DepositRepository : Repository<Deposit>, IDepositRepository
{
    public DepositRepository(AppDbContext context) : base(context)
    {
    }

    public Deposit? GetDepositByUser(int id)
        => _context.Deposits.Include(d => d.User)
                            .FirstOrDefault(d => d.UserId == id);
}
