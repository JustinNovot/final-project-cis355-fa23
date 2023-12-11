using Microsoft.AspNetCore.Mvc;
using UserApi.Authorization;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Authorize(Role = RoleNames.Admin)]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
/// <summary>
    /// Gives information of all the users. This information includes id, first name, last name, username, email, role, activity, date created and last time they logged in. 
    /// </summary>
    /// <response code= "200">User authenticated</response>

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }
/// <summary>
    /// Gives information of a single user. You get this information by pasting their ID into the test. This information includes id, first name, last name, username, email, role, activity, date created and last time they logged in. 
    /// </summary>
    /// <response code= "200">User authenticated</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }
/// <summary>
/// Creates a new user. 
/// Duplicate User<response code= "500">User authenticated</response>
/// Creating New user<response code= "200">User authenticated</response>
/// Missing information <response code= "400">User authenticated</response>

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest newUser)
    {
        var createdUser = await _userService.CreateUserAsync(newUser);
        return Ok(createdUser);
    }
}
