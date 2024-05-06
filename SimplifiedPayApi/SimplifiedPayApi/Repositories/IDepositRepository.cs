using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories;

public interface IDepositRepository
{
    Deposit? GetDepositByUser(int id);
}
