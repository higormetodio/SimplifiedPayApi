using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface ITransactionRepository
{
    Transaction? GetTransactionByWallet(int id);
}
