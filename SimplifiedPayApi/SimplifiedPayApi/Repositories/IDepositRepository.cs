using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface IDepositRepository
{
    ICollection<Deposit>? GetDepositsByUser(int id);
}
