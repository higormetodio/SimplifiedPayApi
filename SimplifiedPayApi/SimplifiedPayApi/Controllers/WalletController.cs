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
[Produces("application/json")]
public class WalletController : Controller
{
    private readonly IRepository<Wallet> _repository;
    private readonly IWalletRepository _repositoryWallet;

    public WalletController(IRepository<Wallet> repository, IWalletRepository reposityWallet)
    {
        _repository = repository;
        _repositoryWallet = reposityWallet;
    }

    /// <summary>
    /// Get a list of Wallet objects
    /// </summary>
    /// <returns>A list of Wallet objects</returns>
    [Authorize]
    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get()
    {
        var users = await _repository.GetAllAsync();

        return Ok(users);
    }

    /// <summary>
    /// Get a list of Wallet objects using pagination
    /// </summary>
    /// <param name="walletsParameters"></param>
    /// <returns>A list of Wallet objects using pagination</returns>
    [HttpGet("pagination")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get([FromQuery] WalletsParameters walletsParameters)
    {
        var wallets = await _repositoryWallet.GetWalletsAsync(walletsParameters);
        return GetWallet(wallets);
    }

    /// <summary>
    /// Get a list of Wallet objects using filter by Full Name
    /// </summary>
    /// <param name="walletFullNameFilter"></param>
    /// <returns>A list of Wallet objects using filter by Full Name</returns>
    [HttpGet("filter/fullname/pagination")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get([FromQuery] WalletFullNameFilter walletFullNameFilter)
    {
        var wallets = await _repositoryWallet.GetWalletFullNameFilterAsync(walletFullNameFilter);
        return GetWallet(wallets);
    }

    /// <summary>
    /// Get a list of Wallet objects using filter by Balance
    /// </summary>
    /// <param name="walletBalanceFilter"></param>
    /// <returns>A list of Wallet objects using filter by Balance</returns>
    [HttpGet("filter/balance/pagination")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<Wallet>>> Get([FromQuery] WalletBalanceFilter walletBalanceFilter)
    {
        var wallets = await _repositoryWallet.GetWalletBalanceFilterAsync(walletBalanceFilter);
        return GetWallet(wallets);
    }

    //Method for organize Json metadate 
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

    /// <summary>
    /// Get a Wallet object by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A Wallet object by Id</returns>
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

    /// <summary>
    /// Create a new Wallet object
    /// </summary>
    /// <remarks>
    /// Request example
    /// POST api/version/wallet
    /// {
    ///     "id": 1,
    ///     "fullName": "Jose",
    ///     "identificationNumber": "99999999999",
    ///     "email": "jose@email.com",
    ///     "password": "12345",
    ///     "balance": 1000,
    ///     "userType": 1
    /// }
    /// </remarks>
    /// <param name="newWallet"></param>
    /// <returns>A new Wallet object created</returns>
    /// <remarks>Return a new Wallet object created</remarks>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult> Post(Wallet newWallet)
    {
        if (newWallet is null)
        {
            return BadRequest();
        }

        var user = await _repository.GetAsync(u => u.IdentificationNumber == newWallet.IdentificationNumber ||
                                        u.Email == newWallet.Email);

        if (user != null)
        {
            throw new DbUpdateException(message: "The Identication Number or Email already registered");
        }

        _repository.Create(newWallet);

        return new CreatedAtRouteResult("BuscarUsuario", new { id = newWallet.Id }, newWallet);
    }

    /// <summary>
    /// Update an existing Wallet object
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newUser"></param>
    /// <returns>A Wallet object updated</returns>
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

    /// <summary>
    /// Delete a Wallet object
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A Wallet object deleted</returns>
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
