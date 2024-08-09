using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTicketSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTicketSystem
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await EnsureTestAdminAsync(userManager);
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[]
            {
                Constants.AdministratorRole,
                Constants.HelpdeskRole,
                Constants.CustomerRole
            };

            foreach (var role in roles)
            {
                var alreadyExists = await roleManager.RoleExistsAsync(role);
                if (!alreadyExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@ticket.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new IdentityUser
            {
                UserName = "admin@ticket.local",
                Email = "admin@ticket.local",
                EmailConfirmed = true // Confirm the email
            };

            var result = await userManager.CreateAsync(testAdmin, "!QAZ@WSX3edc4rfv");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
            }
            else
            {
                // Log errors or handle the case where user creation fails
                // You can use a logger to log the result.Errors
            }
        }
    }
}
