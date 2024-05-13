﻿using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Services;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepositController : Controller
{
    private readonly IRepository<Deposit> _repository;
    private readonly IRepository<Wallet> _repositoryWallet;
    private readonly IDepositRepository _repositoryDeposit;

    public DepositController(IRepository<Deposit> repository, IRepository<Wallet> repositoryWallet, IDepositRepository repositoryDeposit)
    {
        _repository = repository;
        _repositoryWallet = repositoryWallet;
        _repositoryDeposit = repositoryDeposit;
    }

    [HttpGet("wallet/{id:int}")]
    public async Task<ActionResult<Deposit>> GetDepositByDepositor(int id)
    {
        var depositor = await _repositoryDeposit.GetDepositsByWalletAsync(id);

        if (depositor is null)
        {
            return BadRequest("User not found...");
        }

        return Ok(depositor);
    }

    [HttpPost]
    public async Task<ActionResult<Deposit>> Post(Deposit deposit)
    {
        if (deposit is null)
        {
            return BadRequest();
        }

        var wallet = await _repositoryWallet.GetAsync(w => w.Id == deposit.DepositorId);

        if (wallet is null)
        {
            return BadRequest("User not found...");
        }

        _repositoryWallet.Update(WalletService.Credit(wallet, deposit.Amount));
        _repository.Create(deposit);

        return Created();
    }
}
