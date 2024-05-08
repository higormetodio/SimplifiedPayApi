using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Services;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly IRepository<Transaction> _repositoryTransaction;
    private readonly IRepository<Deposit> _repositoryDeposit;
    private readonly IRepository<User> _repositoryUser;
    private readonly ITransactionRepository _transactionRepository;
    private readonly TransactionService _transactionService;

    public TransactionController(IRepository<Transaction> repositoryTransaction, ITransactionRepository transactionRepository,
                                 TransactionService transactionService, IRepository<Deposit> repositoryDeposit, IRepository<User> repositoryUser)
    {
        _repositoryTransaction = repositoryTransaction;
        _transactionRepository = transactionRepository;
        _transactionService = transactionService;
        _repositoryDeposit = repositoryDeposit;
        _repositoryUser = repositoryUser;
    }

    [HttpGet("user/{id:int}")]
    public ActionResult<Transaction> GetTransactionByPayer(int id)
    {
        var transction = _transactionRepository.GetTransactionByUser(id);

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

        var user = _repositoryUser.Get(u => u.Id == transaction.PayerId);

        if (user.UserType == UserType.Shopkeeper)
        {
            return BadRequest("The Payer is a Shopkeeper. Unauthorized transaction");
        }
        
        var payer = _repositoryDeposit.Get(d => d.UserId == transaction.PayerId)!;
        var receiver = _repositoryDeposit.Get(d => d.UserId == transaction.ReceiverId)!;

        _repositoryDeposit.Update(DepositService.Debit(payer, transaction.Amount));
        _repositoryDeposit.Update(DepositService.Credit(receiver, transaction.Amount));

        _repositoryTransaction.Create(transaction);

        string message = await _transactionService.TransactionValidation();

        if (message != "Autorizado")
        {
            _repositoryDeposit.RollBack();
            _repositoryTransaction.RollBack();

            return BadRequest("Unauthorized transaction");
        }

        return Created();
    }
}
