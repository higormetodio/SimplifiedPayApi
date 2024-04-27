using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Repositories
{
    public interface IUserRepository
    {
        User? GetUserByDeposit(int id);

        User? GetUserByTransaction(int id);
    }
}
