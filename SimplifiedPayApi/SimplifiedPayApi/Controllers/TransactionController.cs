using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace SimplifiedPayApi.Controllers;

[Route("api/v{version:apiVersion}/transaction")]
[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
public class TransactionController : Controller
{
    private readonly IRepository<Transaction> _repository;
    private readonly IRepository<Wallet> _repositoryWallet;
    private readonly ITransactionRepository _repositoryTransaction;
    private readonly TransactionService _transactionService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public TransactionController(IRepository<Transaction> repository, IRepository<Wallet> repositoryWallet,
                                 ITransactionRepository repositoryTransaction, TransactionService transactionService, 
                                 IEmailService emailService, IConfiguration configuration)
    {
        _repository = repository;
        _repositoryWallet = repositoryWallet;
        _repositoryTransaction = repositoryTransaction;
        _transactionService = transactionService;
        _emailService = emailService;
        _configuration = configuration;
    }

    /// <summary>
    /// Get a list Transaction objects by Wallet Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A list Transaction objects by Wallet Id</returns>
    [HttpGet("wallet/{id:int}")]
    [Authorize(Policy = "AdminOnly, UserOnly")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByPayerAsync(int id)
    {
        var transactions = await _repositoryTransaction.GetTransactionsByWalletAsync(id);
        var wallet = await _repositoryWallet.GetAsync(w => w.Id == id);

        if (wallet is null)
        {
            return NotFound("User not found...");
        }

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");


        if (loginEmail == wallet!.Email || isAdmin)
        {
            return Ok(transactions);
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }

    /// <summary>
    /// Create a new Transaction object
    /// </summary>
    /// <remarks>
    /// Request example
    /// 
    ///     POST api/version/transaction
    ///     {
    ///         "id": 1,
    ///         "amount": 500,
    ///         "payerId": 1,
    ///         "receiverId": 2,
    ///         "status": true,
    ///         "timestamp": "2024-05-10T20:33:49.996Z"
    ///     }
    /// </remarks>
    /// <param name="transaction"></param>
    /// <returns>A new Transaction object created</returns>
    /// <remarks>Return a new Transaction object created</remarks>
    [HttpPost]
    [Authorize(Policy = "AdminOnly, UserOnly")]
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

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");

        if (loginEmail == payer.Email || isAdmin)
        {
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

            await _emailService.SendEmailAsync(_configuration, receiver.FullName, receiver.Email);

            return Created();
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }
}
