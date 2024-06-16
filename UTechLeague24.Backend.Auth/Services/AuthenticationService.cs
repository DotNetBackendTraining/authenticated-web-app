using AutoMapper;
using UTechLeague24.Backend.Auth.Interfaces;
using UTechLeague24.Backend.Auth.Models;
using UTechLeague24.Backend.Domain.Interfaces;
using UTechLeague24.Backend.Domain.Models;

namespace UTechLeague24.Backend.Auth.Services;

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

    public async Task<AuthenticationResult> CreateUserAsync(RegisterRequest registerRequest)
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

        await _userRepository.AddUserAsync(user);

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult
        {
            Succeeded = true,
            Token = token
        };
    }

    public async Task<AuthenticationResult> SignInAsync(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginRequest.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
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