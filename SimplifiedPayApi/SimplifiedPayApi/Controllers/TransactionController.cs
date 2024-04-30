using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly IRepository<Transaction> _repository;

    public TransactionController(IRepository<Transaction> repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public ActionResult<Transaction> Post(Transaction transaction)
    {
        if (transaction is null)
            return BadRequest();

        _repository.Create(transaction);

        return Created();
    }
}
