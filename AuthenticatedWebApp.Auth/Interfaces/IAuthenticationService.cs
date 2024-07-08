using AuthenticatedWebApp.Auth.Models;
using AuthenticatedWebApp.Domain.Models;

namespace AuthenticatedWebApp.Auth.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResult> CreateUserAsync(RegisterRequest registerRequest, UserRole role);

    Task<AuthenticationResult> SignInAsync(LoginRequest loginRequest);
}