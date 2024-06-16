using UTechLeague24.Backend.Domain.Models;

namespace UTechLeague24.Backend.Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> UserExistsAsync(string username);

    Task<User?> GetUserByIdAsync(string id);

    Task<User?> GetUserByUsernameAsync(string username);

    Task AddUserAsync(User user);
}