using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Services;

public static class DepositService
{
    public static Deposit Debit(Deposit payer, decimal amount)
    {
        if (payer.Amount < amount)
        {
            throw new ArgumentException("The value is more than value in account");
        }
        else
        {
            payer.Amount = payer.Amount - amount;
            
            return payer;
        }

    }

    public static Deposit Credit(Deposit receiver, decimal amount)
    {
            receiver.Amount += amount;

            return receiver;

    }
}
