using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTicketSystem.Models
{
    internal class ManageUsersViewModel
    {
        // Help Items stop complaining about null p. 87
        public ManageUsersViewModel()
        {
            Administrators = Array.Empty<IdentityUser>();
            Everyone = Array.Empty<IdentityUser>();
        }

        public IdentityUser[] Administrators { get; set; }
        public IdentityUser[] Everyone { get; set; }
    }
}