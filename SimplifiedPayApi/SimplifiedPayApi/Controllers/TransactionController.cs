using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Repositories;
using SimplifiedPayApi.Models;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly IRepository<Transaction> _repository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionController(IRepository<Transaction> repository, ITransactionRepository transactionRepository)
    {
        _repository = repository;
        _transactionRepository = transactionRepository;
    }

    [HttpGet("user/{id:int}")]
    public ActionResult<Transaction> GetTransactionByPayer(int id)
    {
        var transction = _transactionRepository.GetTransactionByUser(id);

        if (transction is null)
        {
            return NotFound("User not found...");
        }

        return Ok(transction);
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
