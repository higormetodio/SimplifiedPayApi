using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class DepositRepository : Repository<Deposit>, IDepositRepository
{
    public DepositRepository(AppDbContext context) : base(context)
    {
    }

    public ICollection<Deposit>? GetDepositsByUser(int id)
        => _context.Deposits.Include(d => d.User)
                            .Where(d => d.UserId == id)
                            .ToList();
}
