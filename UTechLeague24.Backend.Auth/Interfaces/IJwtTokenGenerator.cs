using UTechLeague24.Backend.Domain.Models;

namespace UTechLeague24.Backend.Auth.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}