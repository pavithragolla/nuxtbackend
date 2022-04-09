using Dapper;
using task.Repositories;
using Todotask.Models;

namespace Todotask.Repositories;

public interface IUsersRepository
{
    Task<Users> CreateLogin(Users Item);
    Task<Users> Create(Users Item);
    // Task<bool> Update(Users Item);
    Task<Users> GetById(int Id);
    Task<Users> GetByEmail(String Email);
    Task<List<Users>> GetUsers();



}
public class UsersRepository : BaseRepository, IUsersRepository
{
    public UsersRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Users> Create(Users Item)
    {
        var query = $@"INSERT INTO public.users(
	 name, email, password)
	VALUES (@Name, @Email, @Password) RETURNING * ";
        using (var connection = NewConnection)
        {
            var res = await connection.QuerySingleOrDefaultAsync<Users>(query, Item);
            return res;
        }
    }

    public Task<Users> CreateLogin(Users Item)
    {
        throw new NotImplementedException();
    }

    public async Task<Users> GetById(int Id)
    {
        var query = $@"SELECT * FROM users WHERE id = @Id";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { Id });
    }
    public async Task<Users> GetByEmail(String Email)
    {
        var query = $@"SELECT * FROM users WHERE email = @Email";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { Email });
    }

    public async Task<List<Users>> GetUsers()
    {
          var query = $@"SELECT * FROM users";
        List<Users> response;
        using (var con = NewConnection)
            response = (await con.QueryAsync<Users>(query)).AsList();
        return response;
    }

    // public async Task<bool> Update(Users Item)
    // {
    //     var query = $@"UPDATE public.users
	// SET email=@Email, password = @Password
	// WHERE id = @Id";
    //     using (var connection = NewConnection)
    //     {
    //         var Count = await connection.ExecuteAsync(query, Item);
    //         return Count == 1;
    //     }
    // }
    //



}