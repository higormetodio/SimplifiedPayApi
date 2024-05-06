using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context) : base(context)
    {
        
    }

    public Transaction? GetTransactionByUser(int id)
        => _context.Transactions.Include(t => t.Payer)
                                .FirstOrDefault(t => t.PayerId == id);
}
