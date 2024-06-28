using AuthenticatedWebApp.Auth.Models;

namespace AuthenticatedWebApp.Auth.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResult> CreateUserAsync(RegisterRequest registerRequest);
    Task<AuthenticationResult> CreateClientAsync(RegisterRequest registerRequest);
    Task<AuthenticationResult> SignInAsync(LoginRequest loginRequest);
}