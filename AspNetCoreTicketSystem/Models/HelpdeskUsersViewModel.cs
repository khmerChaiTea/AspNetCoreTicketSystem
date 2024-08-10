using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;

namespace AspNetCoreTicketSystem.Models
{
    public class HelpdeskUsersViewModel
    {
        public HelpdeskUsersViewModel()
        {
            HelpdeskUsers = [];
        }

        public IEnumerable<IdentityUser> HelpdeskUsers { get; set; }
    }
}