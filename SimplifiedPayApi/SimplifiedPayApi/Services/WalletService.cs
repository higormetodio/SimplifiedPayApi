using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Services;

public static class WalletService
{
    public static Wallet Debit(Wallet payer, decimal amount)
    {
        if (payer.Balance < amount)
        {
            throw new ArgumentException("The value is more than value in account");
        }
        else
        {
            payer.Balance -= amount;
            
            return payer;
        }

    }

    public static Wallet Credit(Wallet receiver, decimal amount)
    {
            receiver.Balance += amount;

            return receiver;

    }
}
