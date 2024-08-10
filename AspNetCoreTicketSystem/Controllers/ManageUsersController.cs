using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTicketSystem.Models;

namespace AspNetCoreTicketSystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IActionResult> Index()
        {
            var admins = (await _userManager.GetUsersInRoleAsync(Constants.AdministratorRole)).ToArray();
            var everyone = await _userManager.Users.ToArrayAsync();

            var userRoles = new Dictionary<string, IEnumerable<string>>();
            foreach (var user in everyone)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
            }

            var model = new ManageUsersViewModel
            {
                Administrators = admins,
                Everyone = everyone,
                Roles = (await _roleManager.Roles.ToArrayAsync())
                    .Where(r => r.Name != Constants.AdministratorRole) // Exclude Administrator role
                    .ToArray(),
                UserRoles = userRoles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Roles that can be assigned
            var roles = new[] { Constants.HelpdeskRole, Constants.CustomerRole };

            // Get the current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(roleName))
            {
                if (currentRoles.Contains(roleName))
                {
                    // Role is already assigned, so do nothing
                    return RedirectToAction(nameof(Index));
                }

                // Remove user from previous roles and add to the new role
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, roleName);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
