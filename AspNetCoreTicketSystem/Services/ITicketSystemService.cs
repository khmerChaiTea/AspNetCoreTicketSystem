using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTicketSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTicketSystem
{
    public interface ITicketSystemService
    {
        Task<TicketSystem[]> GetAllTicketsAsync();
        Task<TicketSystem> GetTicketByIdAsync(int id);
        Task CreateTicketAsync(TicketSystem ticket);
        Task UpdateTicketAsync(TicketSystem ticket);
        Task DeleteTicketAsync(int id);
    }
}