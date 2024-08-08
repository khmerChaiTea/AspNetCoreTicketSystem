using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTicketSystem.Data;
using AspNetCoreTicketSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTicketSystem.Services
{
    public class TicketSystemService : ITicketSystemService
    {
        private readonly ApplicationDbContext _context;

        public TicketSystemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TicketSystem[]> GetActiveTicketsAsync(IdentityUser user)
        {
            return await _context.Tickets
                                 .Where(t => !t.IsDeleted && t.UserId == user.Id)
                                 .ToArrayAsync();
        }

        public async Task<TicketSystem[]> GetAllTicketsAsync()
        {
            return await _context.Tickets
                                 .OrderBy(t => t.CreatedAt)
                                 .ToArrayAsync();
        }

        public async Task<TicketSystem> GetTicketByIdAsync(int id)
        {
            var ticket = await _context.Tickets
                                       .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

            if (ticket == null)
            {
                throw new InvalidOperationException("Ticket not found");
            }

            return ticket;
        }

        public async Task CreateTicketAsync(TicketSystem ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketAsync(TicketSystem ticket)
        {
            ticket.UpdatedAt = DateTime.UtcNow;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                ticket.IsDeleted = true;
                ticket.UpdatedAt = DateTime.UtcNow;
                _context.Tickets.Update(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
