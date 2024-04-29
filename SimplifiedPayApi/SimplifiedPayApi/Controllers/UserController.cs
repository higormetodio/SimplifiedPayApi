using Microsoft.AspNetCore.Mvc;
using SimplifiedPayApi.Models;
using SimplifiedPayApi.Repositories;

namespace SimplifiedPayApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IRepository<User> _repository;
    private readonly IConfiguration _configuration;

    public UserController(IRepository<User> repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    [HttpGet]
    public ActionResult<IEnumerable<User>> Get()
    {
        var users = _repository.GetAll();

        return Ok(users);
    }

    [HttpGet("{id:int}", Name = "BuscarUsuario")]
    public ActionResult<User> Get(int id)
    {
        var user = _repository.Get(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User not found...");
        }

        return Ok(User);
    }

    [HttpPost]
    public ActionResult Post(User user)
    {
        if (user is null)
            return BadRequest();

        _repository.Create(user);

        return new CreatedAtRouteResult("BuscarUsuario", new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, User user)
    {
        if (id != user.Id)
            return BadRequest();

        _repository.Update(user);

        return Ok(user);
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
