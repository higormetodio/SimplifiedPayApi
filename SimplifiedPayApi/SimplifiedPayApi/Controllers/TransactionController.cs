﻿using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Policy = "AdminOnly, UserOnly")]
    public async Task<ActionResult<Transaction>> GetTransactionsByPayerAsync(int id)
    {
        var transaction = await _repositoryTransaction.GetTransactionsByWalletAsync(id);
        var wallet = await _repositoryWallet.GetAsync(w => w.Id == id);

        if (transaction is null)
        {
            return NotFound("User not found...");
        }

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");

        if (loginEmail == wallet!.Email || isAdmin)
        {
            return Ok(transaction);
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }

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

            return Created();
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }
}
