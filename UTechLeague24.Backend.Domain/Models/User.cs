namespace UTechLeague24.Backend.Domain.Models;

public class User
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public UserRole UserRole { get; set; } = UserRole.User;
}