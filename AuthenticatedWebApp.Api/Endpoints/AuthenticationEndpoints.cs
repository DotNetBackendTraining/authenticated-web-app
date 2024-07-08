using AuthenticatedWebApp.Auth.Interfaces;
using AuthenticatedWebApp.Auth.Models;
using AuthenticatedWebApp.Domain.Models;

namespace AuthenticatedWebApp.Api.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/auth/register",
                async (RegisterRequest registerRequest, IAuthenticationService authenticationService) =>
                {
                    var result = await authenticationService.CreateUserAsync(registerRequest, UserRole.User);
                    return result.Succeeded
                        ? Results.Ok(result)
                        : Results.Conflict(result);
                })
            .Produces<AuthenticationResult>()
            .Produces<AuthenticationResult>(StatusCodes.Status409Conflict)
            .WithName("RegisterUser");

        endpoints.MapPost("/api/auth/login",
                async (LoginRequest loginRequest, IAuthenticationService authenticationService) =>
                {
                    var result = await authenticationService.SignInAsync(loginRequest);
                    return result.Succeeded
                        ? Results.Ok(result)
                        : Results.Unauthorized();
                })
            .Produces<AuthenticationResult>()
            .Produces<AuthenticationResult>(StatusCodes.Status401Unauthorized)
            .WithName("LoginUser");
    }
}