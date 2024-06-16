using UTechLeague24.Backend.Auth.Models;

namespace UTechLeague24.Backend.Auth.Interfaces;

public interface IAuthenticationService
{
    Task<AuthenticationResult> CreateUserAsync(RegisterRequest registerRequest);
    Task<AuthenticationResult> CreateClientAsync(RegisterRequest registerRequest);
    Task<AuthenticationResult> SignInAsync(LoginRequest loginRequest);
}