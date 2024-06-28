using AuthenticatedWebApp.Domain.Models;

namespace AuthenticatedWebApp.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(string username);

    Task<User?> GetUserByIdAsync(string id);

    Task<User?> GetUserByUsernameAsync(string username);

    Task AddUserAsync(User user);
}