using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTicketSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTicketSystem
{
    public interface ITicketSystemService
    {
        Task<TicketSystem[]> GetActiveTicketsAsync(IdentityUser user); // Add this line
        Task<TicketSystem[]> GetAllTicketsAsync(); // Ensure this line is present
        Task<TicketSystem?> GetTicketByIdAsync(int id);
        Task CreateTicketAsync(TicketSystem ticket);
        Task UpdateTicketAsync(TicketSystem ticket);
        Task DeleteTicketAsync(int id);
    }
}

