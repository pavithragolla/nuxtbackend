using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todotask.DTOs;
using Todotask.Models;
using Todotask.Repositories;

namespace Todotask.Controllers;

[ApiController]
[Route("api/todos")]

public class TodosController : ControllerBase
{
    private readonly ILogger<TodosController> _logger;
    private readonly ITodosRepository _Todos;

    public TodosController(ILogger<TodosController> logger, ITodosRepository Todos)
    {
        _logger = logger;
        _Todos = Todos;
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TodosDTO>> CreateTodos([FromBody] TodosCreateDTO Data)
    {
        var ToCreateTodos = new Todos
        {
            UserId = Data.UserId,
            Description = Data.Description,
            Title = Data.Title
        };
        var CreatedTodos = await _Todos.Create(ToCreateTodos);

        return StatusCode(StatusCodes.Status201Created, CreatedTodos.asDto);
    }
    [HttpGet("alltodos")]
    [Authorize]
    public async Task<ActionResult<List<TodosDTO>>> GetAllTodos()
    {
        var TodosList = await _Todos.GetTodos();

        return Ok(TodosList);
    }

    [HttpGet("getmytodos")]
    [Authorize]
    public async Task<ActionResult<List<Todos>>> GetMyTodos()
    {
        var id = GetCurrentUserId();
        var Todos = await _Todos.GetMyTodos(Int32.Parse(id));

        return Ok(Todos);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<TodosDTO>> GetTodosById([FromRoute] int id)
    {

        var Todos = await _Todos.GetById(id);

        if (Todos is null)
            return NotFound("No Todos found with given Todos_id");


        var dto = Todos.asDto;
        // dto.Students = (await _student.GetAllForClass(id)).Select(x => x.asDto).ToList();

        // dto.Product = (await _product.GetAllForTodos(Todos_id)).Select(x => x.asDto).ToList();
        // dto.Order = (await _order.GetAllForTodos(Todos_id)).Select(x => x.asDto).ToList();
        return Ok(dto);
    }
    [HttpPut("{id}")]
    [Authorize]

    public async Task<ActionResult> UpdateTodos(int id, [FromBody] TodosUpdateDTO Data)
    {
        var existing = await _Todos.GetById(id);
        var currentUserId = GetCurrentUserId();
        if (Int32.Parse(currentUserId) != existing.UserId)
            return Unauthorized("Your are not Authorized");


        if (existing is null)
            return NotFound("No Todos found with given Todos Id");

        var toUpdateTodos = existing with
        {
            Description = Data.Description ?? existing.Description,
            Title = Data.Title ?? existing.Title
        };

        var didUpdate = await _Todos.Update(toUpdateTodos);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Todos");

        return NoContent();
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteTodos([FromRoute] int id)
    {

        var todos = await _Todos.GetById(id);
        var currentUser = GetCurrentUserId();
        if (Int32.Parse(currentUser) != todos.UserId)
            return Unauthorized("Your not Authorized to delete");

        if (todos == null)
            return NotFound("Todo Not found");
        var didDelete = await _Todos.Delete(id);
        if (!didDelete)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete");
        return Ok("Deleted");



        // var existing = await _Todos.GetById(id);
        // if (existing is null)
        //     return NotFound("No user found with given Todos id");

        // var didDelete = await _Todos.Delete(id);

        // return NoContent();
    }

    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userClaim = identity.Claims;
        return (userClaim.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
    }

}