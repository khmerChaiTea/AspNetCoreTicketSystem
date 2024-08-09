using Microsoft.AspNetCore.Identity;

public class ManageUsersViewModel
{
    public ManageUsersViewModel()
    {
        Administrators = Array.Empty<IdentityUser>();
        HelpdeskUsers = Array.Empty<IdentityUser>();
        Everyone = Array.Empty<IdentityUser>();
        Roles = Array.Empty<IdentityRole>();
        UserRoles = new Dictionary<string, IEnumerable<string>>(); // Add this line
    }

    public IEnumerable<IdentityUser> Administrators { get; set; }
    public IEnumerable<IdentityUser> HelpdeskUsers { get; set; }
    public IEnumerable<IdentityUser> Everyone { get; set; }
    public IEnumerable<IdentityRole> Roles { get; set; }
    public IDictionary<string, IEnumerable<string>> UserRoles { get; set; } // Add this line
}
