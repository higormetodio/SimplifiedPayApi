using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface IDepositRepository
{
    Task <ICollection<Deposit>?> GetDepositsByWalletAsync(int id);
}
