using System.Text.Json.Serialization;

namespace Todotask.DTOs;

public record UsersDTO
{
     [JsonPropertyName("id")]
     public int Id { get; set; }

     [JsonPropertyName("name")]
    public String Name { get; set; }

     [JsonPropertyName("email")]
    public String Email { get; set; }
     [JsonPropertyName("password")]
    public String Password { get; set; }
}
public record UsersCreateDTO
{


     [JsonPropertyName("name")]
    public String Name { get; set; }

     [JsonPropertyName("email")]
    public String Email { get; set; }
     [JsonPropertyName("password")]
    public String Password { get; set; }
}
public record UsersLoginDTO
{
     [JsonPropertyName("email")]
    public String Email { get; set; }
     [JsonPropertyName("password")]
    public String Password { get; set; }
}