using Microsoft.AspNetCore.Mvc;
using UTechLeague24.Backend.Auth.Interfaces;
using UTechLeague24.Backend.Auth.Models;

namespace UTechLeague24.Backend.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResult>> Register([FromBody] RegisterRequest registerRequest)
    {
        var result = await _authenticationService.CreateUserAsync(registerRequest);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return Conflict(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginRequest loginRequest)
    {
        var result = await _authenticationService.SignInAsync(loginRequest);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return Unauthorized(result);
    }
}