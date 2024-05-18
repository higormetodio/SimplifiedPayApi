using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface IDepositRepository
{
    Task <IEnumerable<Deposit>?> GetDepositsByWalletAsync(int id);
}
