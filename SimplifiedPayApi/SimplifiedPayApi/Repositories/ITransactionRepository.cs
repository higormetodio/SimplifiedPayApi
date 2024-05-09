using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface ITransactionRepository
{
    ICollection<Transaction>? GetTransactionsByWallet(int id);
}
