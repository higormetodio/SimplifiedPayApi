using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepositController : Controller
{
    private readonly IRepository<Deposit> _repository;

    public DepositController(IRepository<Deposit> repository)
    {
        _repository = repository;
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
