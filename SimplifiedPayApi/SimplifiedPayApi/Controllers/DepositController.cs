using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;
using System.Net;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepositController : Controller
{
    private readonly IRepository<Deposit> _repository;
    private readonly IDepositRepository _depositRepository;

    public DepositController(IRepository<Deposit> repository, IDepositRepository depositRepository)
    {
        _repository = repository;
        _depositRepository = depositRepository;
    }

    [HttpGet("user/{id:int}")]
    public ActionResult<ICollection<Deposit>> GetDepositByUser(int id)
    {
        var deposits = _depositRepository.GetDepositsByUser(id);

        if (deposits is null)
        {
            return NotFound("User not found");
        }

        return Ok(deposits);
    }

    [HttpPost]
    public ActionResult<Deposit> Post(Deposit deposit)
    {
        if (deposit is null)
        {
            return BadRequest();
        }

        var amoutDeposit = _repository.Get(d => d.UserId == deposit.UserId)!;

        if (amoutDeposit.Amount > 0)
        {
            amoutDeposit.Amount += deposit.Amount;
        }

        _repository.Create(deposit);

        return Created();
    }
}
