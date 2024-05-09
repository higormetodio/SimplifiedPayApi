using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;

namespace SimplifiedPayApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings()]
public class WalletController : Controller
{
    private readonly IRepository<Wallet> _repository;

    public WalletController(IRepository<Wallet> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Wallet>> Get()
    {
        var users = _repository.GetAll();

        return Ok(users);
    }

    [HttpGet("{id:int}", Name = "BuscarUsuario")]
    public ActionResult<Wallet> Get(int id)
    {
        var user = _repository.Get(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User not found...");
        }

        return Ok(user);
    }

    [HttpPost]
    public ActionResult Post(Wallet newUser)
    {
        if (newUser is null)
        {
            return BadRequest();
        }

        var user = _repository.Get(u => u.IdentificationNumber == newUser.IdentificationNumber ||
                                        u.Email == newUser.Email);

        if (user != null)
        {
            throw new DbUpdateException(message: "The Identication Number or Email already registered");
        }

        _repository.Create(newUser);

        return new CreatedAtRouteResult("BuscarUsuario", new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Wallet newUser)
    {
        if (id != newUser.Id)
            return BadRequest();

        var user = _repository.Get(u => u.IdentificationNumber == newUser.IdentificationNumber ||
                                        u.Email == newUser.Email);

        _repository.Update(newUser);

        return Ok(newUser);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var user = _repository.Get(u => u.Id == id);

        if (user is null)
            return NotFound("User not found...");

        _repository.Delete(user);

        return Ok(user);
    }


}
