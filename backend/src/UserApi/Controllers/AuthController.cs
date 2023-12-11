using Microsoft.AspNetCore.Mvc;
using UserApi.Authorization;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// Allows the user to authenticate with username and password
    /// </summary>
    /// <response code= "200">User authenticated</response>
    /// <response code="400">Username or password is incorrect</response>
    [AllowAnonymous]
    [HttpPost("login")]
    
    public async Task<IActionResult> Login(AuthenticateRequest model)
    {
        var response = await _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(response);
    }
}
