using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Services;
using System.Security.Claims;

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
    [Authorize(Policy = "AdminOnly, UserOnly")]
    public async Task<ActionResult<Deposit>> GetDepositsByDepositor(int id)
    {
        var depositor = await _repositoryDeposit.GetDepositsByWalletAsync(id);
        var wallet = await _repositoryWallet.GetAsync(w => w.Id == id);

        if (depositor is null)
        {
            return BadRequest("User not found...");
        }

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");

        if (loginEmail == wallet!.Email || isAdmin)
        {
            return Ok(depositor);
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly, UserOnly")]
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

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");

        if (loginEmail == wallet!.Email || isAdmin)
        {
            _repositoryWallet.Update(WalletService.Credit(wallet, deposit.Amount));
            _repository.Create(deposit);

            return Created();
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }
}
