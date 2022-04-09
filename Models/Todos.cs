using Todotask.DTOs;

namespace Todotask.Models;

public record Todos
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public String Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public String Title { get; set; }

    public TodosDTO asDto => new TodosDTO
    {
           UserId = @UserId,
            Description = @Description,
            Title = @Title
    };
}
