using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface IDepositRepository
{
    ICollection<Deposit>? GetDepositsByWallet(int id);
}
