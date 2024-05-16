using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Pagination;
using SimplifiedPayApi.Repositories;
using System.Security.Claims;
using System.Text.Json;

namespace SimplifiedPayApi.Controllers;

[Route("api/v{version:apiVersion}/wallet")]
[ApiController]
[ApiVersion("1.0")]
public class WalletController : Controller
{
    private readonly IRepository<Wallet> _repository;
    private readonly IWalletRepository _repositoryWallet;

    public WalletController(IRepository<Wallet> repository, IWalletRepository reposityWallet)
    {
        _repository = repository;
        _repositoryWallet = reposityWallet;
    }

    [Authorize]
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get()
    {
        var users = await _repository.GetAllAsync();

        return Ok(users);
    }

    [HttpGet("pagination")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get([FromQuery] WalletsParameters walletsParameters)
    {
        var wallets = await _repositoryWallet.GetWalletsAsync(walletsParameters);
        return GetWallet(wallets);
    }

    [HttpGet("filter/fullname/pagination")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get([FromQuery] WalletFullNameFilter walletFullNameFilter)
    {
        var wallets = await _repositoryWallet.GetWalletFullNameFilterAsync(walletFullNameFilter);
        return GetWallet(wallets);
    }

    [HttpGet("filter/balance/pagination")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get([FromQuery] WalletBalanceFilter walletBalanceFilter)
    {
        var wallets = await _repositoryWallet.GetWalletBalanceFilterAsync(walletBalanceFilter);
        return GetWallet(wallets);
    }

    private ActionResult<IEnumerable<Wallet>> GetWallet(PagedList<Wallet> wallets)
    {
        var metadata = new
        {
            wallets.TotalCount,
            wallets.PageSize,
            wallets.CurrentPage,
            wallets.TotalPages,
            wallets.HasNext,
            wallets.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

        return Ok(wallets);
    }
   
    [HttpGet("{id:int}", Name = "BuscarUsuario")]
    [Authorize(Policy = "AdminOnly, UserOnly")]
    public async Task<ActionResult<Wallet>> Get(int id)
    {
        var user = await _repository.GetAsync(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User not found...");
        }

        var loginEmail = User.FindFirst(ClaimTypes.Email)!.Value;
        var isAdmin = User.IsInRole("Admin");

        if (loginEmail == user.Email || isAdmin)
        {
            return Ok(user);
        }

        return Unauthorized(new Response { Status = "Erro", Message = "Unauthorized user" });

        
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> Post(Wallet newUser)
    {
        if (newUser is null)
        {
            return BadRequest();
        }

        var user = await _repository.GetAsync(u => u.IdentificationNumber == newUser.IdentificationNumber ||
                                        u.Email == newUser.Email);

        if (user != null)
        {
            throw new DbUpdateException(message: "The Identication Number or Email already registered");
        }

        _repository.Create(newUser);

        return new CreatedAtRouteResult("BuscarUsuario", new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> Put(int id, Wallet newUser)
    {
        if (id != newUser.Id)
            return BadRequest();

        var user = await _repository.GetAsync(u => u.IdentificationNumber == newUser.IdentificationNumber ||
                                        u.Email == newUser.Email);

        _repository.Update(newUser);

        return Ok(newUser);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> Delete(int id)
    {
        var user = await _repository.GetAsync(u => u.Id == id);

        if (user is null)
            return NotFound("User not found...");

        _repository.Delete(user);

        return Ok(user);
    }


}
