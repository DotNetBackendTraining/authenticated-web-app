namespace AuthenticatedWebApp.Auth.Interfaces;

public interface IUserSeeder
{
    Task SeedClientUsersAsync();
}