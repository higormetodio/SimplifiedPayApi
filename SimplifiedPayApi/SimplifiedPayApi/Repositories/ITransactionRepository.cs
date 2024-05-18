using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>?> GetTransactionsByWalletAsync(int id);
}
