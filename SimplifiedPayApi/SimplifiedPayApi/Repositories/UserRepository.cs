using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public User? GetUserByDeposit(int id) 
        => _context.Users.Include(u => u.Deposits)
                         .FirstOrDefault(u => u.Id == id);

    public User? GetUserByTransaction(int id)
        => _context.Users.Include(u => u.Transactions)
                         .FirstOrDefault(u => u.Id == id);
}
