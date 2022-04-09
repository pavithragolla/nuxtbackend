using Dapper;
using task.Repositories;
using Todotask.Models;

namespace Todotask.Repositories;

public interface ITodosRepository
{
    Task<Todos> Create(Todos Item);
    Task<bool> Update(Todos Item);
    Task<Todos> GetById(int Id);
    Task<List<Todos>> GetTodos();
    Task<List<Todos>> GetMyTodos(int UserId);
    Task<bool> Delete(int Id);
}
public class TodosRepository : BaseRepository, ITodosRepository
{
    public TodosRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Todos> Create(Todos Item)
    {
         var query = $@"INSERT INTO public.todos(
	 user_id, description, title)
	VALUES (@UserId, @Description, @title) RETURNING * ";
        using (var connection = NewConnection)
        {
            var res = await connection.QuerySingleOrDefaultAsync<Todos>(query, Item);
            return res;
        }
    }

//     public async Task Create(Todos toCreateTodos)
//     {
//   var query = $@"INSERT INTO public.todos(
// 	 user_id, description,title)
// 	VALUES (@UserId,@Description,@title) RETURNING * ";
//         using (var connection = NewConnection)
//         {
//             var res = await connection.QuerySingleOrDefaultAsync<Todos>(query, Item);
//             return res;
//         }
//     }

    public async Task<bool> Delete(int Id)
    {
        var query = $@"DELETE FROM todos WHERE id=@Id";

        using (var connection = NewConnection)
        {
            var res = await connection.ExecuteAsync(query, new { Id });
            return res > 0;
        }
    }

    public async Task<Todos> GetById(int Id)
    {
         var query = $@"SELECT * FROM todos WHERE id = @Id";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Todos>(query, new { Id });
    }

    public async Task<List<Todos>> GetTodos()
    {
        var query = $@"SELECT * FROM todos ORDER BY ID";
        List<Todos> response;
        using (var con = NewConnection)
            response = (await con.QueryAsync<Todos>(query)).AsList();
        return response;
    }
     public async Task<List<Todos>> GetMyTodos(int UserId)
    {
        var query = $@"SELECT * FROM todos WHERE user_id = @UserId";
        // List<Todos> response;
        using (var con = NewConnection)
        return  (await con.QueryAsync<Todos>(query, new { UserId })).ToList();

    }

    public async Task<bool> Update(Todos Item)
    {
       var query = $@"UPDATE public.todos
	SET description =@Description, title = @Title
	WHERE id = @Id";
        using (var connection = NewConnection)
        {
            var Count = await connection.ExecuteAsync(query, Item);
            return Count == 1;
        }
    }
}