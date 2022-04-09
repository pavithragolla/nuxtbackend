using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todotask.DTOs;
using Todotask.Models;
using Todotask.Repositories;

namespace Todotask.Controllers;

[ApiController]
[Route("api/users")]

public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _users;
    private IConfiguration _configuration;

    public UsersController(ILogger<UsersController> logger, IUsersRepository users, IConfiguration configuration)
    {
        _logger = logger;
        _users = users;
        _configuration = configuration;
    }
    [HttpPost]

    public async Task<ActionResult<UsersDTO>> CreateUsers([FromBody] UsersCreateDTO Data)
    {
        var ToCreateUsers = new Users
        {
            Name = Data.Name,
            Email = Data.Email,
            Password = Data.Password
        };
        var CreatedUsers = await _users.Create(ToCreateUsers);

        return StatusCode(StatusCodes.Status201Created, CreatedUsers.asDto);
    }
    [HttpGet]
    public async Task<ActionResult<List<UsersDTO>>> GetAllUsers()
    {
        var usersList = await _users.GetUsers();


        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<UsersDTO>> GetUserById([FromRoute] int id)
    {

        var Users = await _users.GetById(id);

        if (Users is null)
            return NotFound("No Users found with given Users_id");


        var dto = Users.asDto;
        // dto.Students = (await _student.GetAllForClass(id)).Select(x => x.asDto).ToList();

        // dto.Product = (await _product.GetAllForUsers(Users_id)).Select(x => x.asDto).ToList();
        // dto.Order = (await _order.GetAllForUsers(Users_id)).Select(x => x.asDto).ToList();
        return Ok(dto);
    }
    //     [HttpPut("{id}")]

    // public async Task<ActionResult> UpdateUsers([FromRoute] int id,
    // [FromBody] UsersUpdateDTO Data)
    // {
    //     var existing = await _users.GetById (id);
    //     if (existing is null)
    //         return NotFound("No Users found with given Users Id");

    //     var toUpdateUsers = existing with
    //     {
    //         Email = Data.Email,
    //         Password= Data.Password

    //     };

    //     var didUpdate = await _users.Update(toUpdateUsers);

    //     if (!didUpdate)
    //         return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Users");

    //     return NoContent();
    // }

    // [HttpPost("{login}")]

    // public async Task<ActionResult<UsersDTO>> CreateLogin([FromBody]UsersLoginDTO Data)
    // {
    //     var ToCreateUsers = new Users
    //     {
    //        Email = Data.Email,
    //        Password = Data.Password,
    //     };
    //      var CreatedUsers = await _users.Create(ToCreateUsers);

    //     return StatusCode(StatusCodes.Status201Created, CreatedUsers.asDto);
    // }


    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<UsersDTO>> Login([FromBody] UserLogin userLogin)
    {
        // var user = Authenticate(userLogin);
        // if (user != null)
        // {
        //     var token = Generate(user);
        //     return Ok(token);
        // }

        // return NotFound("User not found");
        var User = await _users.GetByEmail(userLogin.Email);
        if (User == null)
            return NotFound("No user Found");


        if (User.Password != userLogin.Password)
            return Unauthorized("Involid password");

        var token = Generate(User);
        return Ok(token);

    }

    private String Generate(Users user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            // new Claim(ClaimTypes.Password, user.Password),
            // new Claim(ClaimTypes.Surname, user.Surname),
            // new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // private Users Authenticate(UserLogin userLogin)
    //  {
    //     // List<Users> Users = _users GetByName();
    //     // var currentUser =Users.FirstOrDefault(o => o.Name.ToLower() ==
    //     // userLogin.Email.ToLower() && o.Password == userLogin.Password);


    //     var currentUser = Users.GetByName();
    //     if (currentUser != null)
    //     {
    //         return currentUser;
    //     }

    //     return null;

    // }



}