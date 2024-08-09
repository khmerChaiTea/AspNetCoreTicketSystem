using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;

namespace AspNetCoreTicketSystem.Models
{
    public class HelpdeskUsersViewModel
    {
        public HelpdeskUsersViewModel()
        {
            HelpdeskUsers = Array.Empty<IdentityUser>();
        }

        public IEnumerable<IdentityUser> HelpdeskUsers { get; set; }
    }
}