using UTechLeague24.Backend.Auth.Interfaces;

namespace UTechLeague24.Backend.Api.Configurations;

public static class StartupTasksExtension
{
    public static async Task SeedClientUsersAsync(this IServiceScope scope)
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IUserSeeder>();
        await seeder.SeedClientUsersAsync();
    }
}