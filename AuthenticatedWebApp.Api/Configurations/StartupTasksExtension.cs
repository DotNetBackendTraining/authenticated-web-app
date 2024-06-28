using AuthenticatedWebApp.Auth.Interfaces;

namespace AuthenticatedWebApp.Api.Configurations;

public static class StartupTasksExtension
{
    public static async Task SeedClientUsersAsync(this IServiceScope scope)
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
        await seeder.SeedClientUsersAsync();
    }
}