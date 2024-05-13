using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<ICollection<Transaction>?> GetTransactionsByWalletAsync(int id)
    {
        var transactions = await GetAllAsync();
        
        return transactions.Where(t => t.PayerId == id).ToList();
    }
}
