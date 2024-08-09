using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreTicketSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTicketSystem; // Ensure this namespace is included for Constants

public class HelpdeskUsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public HelpdeskUsersController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(Constants.HelpdeskRole);

        var model = new HelpdeskUsersViewModel
        {
            HelpdeskUsers = usersInRole
        };

        return View(model);
    }
}
