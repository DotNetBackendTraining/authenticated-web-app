using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UTechLeague24.Backend.Auth.Interfaces;
using UTechLeague24.Backend.Auth.Models;
using UTechLeague24.Backend.Auth.Settings;
using UTechLeague24.Backend.Domain.Interfaces;
using UTechLeague24.Backend.Domain.Models;

namespace UTechLeague24.Backend.Auth.Services;

public class UserSeeder : IUserSeeder
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ClientUserSettings _settings;
    private readonly ILogger<UserSeeder> _logger;

    public UserSeeder(
        IUserRepository userRepository,
        IAuthenticationService authenticationService,
        IOptions<ClientUserSettings> options,
        ILogger<UserSeeder> logger)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
        _settings = options.Value ?? throw new ArgumentException("ClientUserSettings");
        _logger = logger;
    }

    public async Task SeedClientUsersAsync()
    {
        foreach (var clientUser in _settings.ClientUsers)
        {
            var user = await _userRepository.GetUserByUsernameAsync(clientUser.Username);
            if (user != null)
            {
                if (user.UserRole == UserRole.Client)
                {
                    _logger.LogInformation($"{clientUser.Username} is registered as a client");
                    continue;
                }

                throw new InvalidOperationException($"{clientUser.Username} is already registered as non client");
            }

            var registerRequest = new RegisterRequest
            {
                Username = clientUser.Username,
                Password = clientUser.Password
            };

            var result = await _authenticationService.CreateClientAsync(registerRequest);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Couldn't add client: {clientUser.Username}");
            }

            _logger.LogInformation($"{clientUser.Username} has been added as a client");
        }
    }
}