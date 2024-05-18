using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Services;
using System.Security.Claims;

namespace SimplifiedPayApi.Controllers;

[Route("api/v{version:apiVersion}/deposit")]
[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
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

    /// <summary>
    /// Get a list Deposit objects by Wallet Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A list Deposit objects by Wallet Id</returns>
    [HttpGet("wallet/{id:int}")]
    [Authorize(Policy = "AdminOnly, UserOnly")]
    public async Task<ActionResult<IEnumerable<Deposit>>> GetDepositsByDepositor(int id)
    {
        var deposits = await _repositoryDeposit.GetDepositsByWalletAsync(id);
        var wallet = await _repositoryWallet.GetAsync(w => w.Id == id);

        if (deposits is null)
        {
            return BadRequest("User not found...");
        }

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");

        if (loginEmail == wallet!.Email || isAdmin)
        {
            return Ok(deposits);
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });
    }

    /// <summary>
    /// Create a new Deposit object
    /// </summary>
    /// <remarks>
    /// Request example
    /// POST api/version/transaction
    /// {
    ///     "id": 1,
    ///     "amount": 500,
    ///     "depositorId": 2,
    ///     "timeSpan": "2024-05-10T20:50:45.390Z"
    /// }
    /// </remarks>
    /// <param name="deposit"></param>
    /// <returns>A new Deposit object created</returns>
    /// <remarks>Return a new Deposit object created</remarks>
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
