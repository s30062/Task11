using Task11.API;
using Task11.Repository.context;

namespace Task11.Services.services;

public static class RoleSeeder
{
    private static readonly string[] DefaultRoles = { "Admin", "User" };

    public static async Task SeedRolesAsync(EmployeeDatabaseContext context)
    {
        foreach (var roleName in DefaultRoles)
        {
            if (!context.Roles.Any(r => r.Name == roleName))
            {
                context.Roles.Add(new Role { Name = roleName });
            }
        }

        await context.SaveChangesAsync();
    }
}