using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Context;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context) : base(context)
    {
        
    }

    public ICollection<Transaction>? GetTransactionsByWallet(int id)
        => GetAll().Where(t => t.PayerId == id).ToList();
}
