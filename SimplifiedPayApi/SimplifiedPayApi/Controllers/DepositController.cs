using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;

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
    public ActionResult<Deposit> GetDepositByUser(int id)
    {
        var deposit = _depositRepository.GetDepositByUser(id);

        if (deposit is null)
        {
            return NotFound("User not found");
        }

        return Ok(deposit);
    }

    [HttpPost]
    public ActionResult<Deposit> Post(Deposit deposit)
    {
        if (deposit is null)
            return BadRequest();

        _repository.Create(deposit);

        return Created();
    }
}
