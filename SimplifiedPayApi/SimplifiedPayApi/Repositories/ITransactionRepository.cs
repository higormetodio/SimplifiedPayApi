using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface ITransactionRepository
{
    Transaction? GetTransactionByUser(int id);
}
