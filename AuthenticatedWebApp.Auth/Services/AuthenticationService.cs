using AuthenticatedWebApp.Auth.Interfaces;
using AuthenticatedWebApp.Auth.Models;
using AuthenticatedWebApp.Domain.Interfaces;
using AuthenticatedWebApp.Domain.Models;
using AutoMapper;

namespace AuthenticatedWebApp.Auth.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(
        IMapper mapper,
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResult> CreateUserAsync(RegisterRequest registerRequest, UserRole role)
    {
        if (await _userRepository.UserExistsAsync(registerRequest.Username))
        {
            return new AuthenticationResult
            {
                Succeeded = false,
                Errors = new List<string> { "Username already exists." }
            };
        }

        var user = _mapper.Map<User>(registerRequest);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
        user.UserRole = role;

        await _userRepository.AddUserAsync(user);
        return new AuthenticationResult
        {
            Succeeded = true,
            Token = _jwtTokenGenerator.GenerateToken(user)
        };
    }

    public async Task<AuthenticationResult> SignInAsync(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);

        var authenticatedUser = user != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);
        if (!authenticatedUser)
        {
            return new AuthenticationResult
            {
                Succeeded = false,
                Errors = new List<string> { "Invalid credentials." }
            };
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult
        {
            Succeeded = true,
            Token = token
        };
    }
}