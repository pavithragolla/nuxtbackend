using Todotask.DTOs;

namespace Todotask.Models;

public record Users
{
    public int Id { get; set; }
    public String Name { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }


      public UsersDTO asDto => new UsersDTO
    {
             Name = @Name,
            Email = @Email,
            Password = @Password
    };
}