using AuthenticatedWebApp.Domain.Models;

namespace AuthenticatedWebApp.Auth.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}