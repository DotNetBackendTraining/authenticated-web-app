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

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerRequest">The registration information.</param>
    /// <returns>An ActionResult containing the authentication result.</returns>
    /// <response code="200">Returns the authentication result if registration is successful.</response>
    /// <response code="409">Returns the authentication result if registration fails (e.g., user already exists).</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthenticationResult>> Register([FromBody] RegisterRequest registerRequest)
    {
        var result = await _authenticationService.CreateUserAsync(registerRequest);
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return Conflict(result);
    }

    /// <summary>
    /// Authenticates a user.
    /// </summary>
    /// <param name="loginRequest">The login credentials.</param>
    /// <returns>An ActionResult containing the authentication result.</returns>
    /// <response code="200">Returns the authentication result if login is successful.</response>
    /// <response code="401">Returns the authentication result if login fails.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status401Unauthorized)]
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