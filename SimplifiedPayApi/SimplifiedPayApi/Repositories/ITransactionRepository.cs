using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface ITransactionRepository
{
    Task<ICollection<Transaction>?> GetTransactionsByWalletAsync(int id);
}
