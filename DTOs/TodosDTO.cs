namespace Todotask.DTOs;

public record TodosDTO
{
     public int Id { get; set; }
    public int UserId { get; set; }
    public String Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public String Title { get; set; }
}
public record TodosCreateDTO
{

    public int UserId { get; set; }
    public String Description { get; set; }
    public String Title { get; set; }
}
public record TodosUpdateDTO
{
    public String Description { get; set; }
    public String Title { get; set; }
}