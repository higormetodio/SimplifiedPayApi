using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Services;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly IRepository<Transaction> _repository;
    private readonly IRepository<Wallet> _repositoryWallet;
    private readonly ITransactionRepository _repositoryTransaction;
    private readonly TransactionService _transactionService;

    public TransactionController(IRepository<Transaction> repository, IRepository<Wallet> repositoryWallet, 
                                 ITransactionRepository repositoryTransaction, TransactionService transactionService)
    {
        _repository = repository;
        _repositoryWallet = repositoryWallet;
        _repositoryTransaction = repositoryTransaction;
        _transactionService = transactionService;
    }

    [HttpGet("wallet/{id:int}")]
    public async Task<ActionResult<Transaction>> GetTransactionByPayerAsync(int id)
    {
        var transction = await _repositoryTransaction.GetTransactionsByWalletAsync(id);

        if (transction is null)
        {
            return NotFound("User not found...");
        }

        return Ok(transction);
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> Post(Transaction transaction)
    {
        if (transaction is null)
            return BadRequest();

        if (transaction.PayerId == transaction.ReceiverId)
            return BadRequest("The Payer and Receiver can't the same.");

        var payer = await _repositoryWallet.GetAsync(w => w.Id == transaction.PayerId);

        if (payer is null)
            return BadRequest("User not found...");

        if (payer.UserType == UserType.Shopkeeper)
        {
            return BadRequest("The Payer is a Shopkeeper. Unauthorized transaction");
        }
        
        var receiver = await _repositoryWallet.GetAsync(w => w.Id == transaction.ReceiverId)!;

        _repositoryWallet.Update(WalletService.Debit(payer, transaction.Amount));
        _repositoryWallet.Update(WalletService.Credit(receiver, transaction.Amount));

        _repository.Create(transaction);

        string message = await _transactionService.TransactionValidation();

        if (message != "Autorizado")
        {
            _repositoryWallet.RollBack();
            _repository.RollBack();

            return BadRequest("Unauthorized transaction");
        }

        return Created();
    }
}
