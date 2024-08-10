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
    public class TicketSystemService(ApplicationDbContext context) : ITicketSystemService
    {
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<TicketSystem[]> GetActiveTicketsAsync(IdentityUser user)
        {
            return await _context.Tickets
                                 .Where(t => !t.IsDeleted && t.UserId == user.Id)
                                 .ToArrayAsync();
        }

        public async Task<TicketSystem[]> GetAllTicketsAsync()
        {
            return await _context.Tickets
                                 .OrderByDescending(t => t.CreatedAt)
                                 .ToArrayAsync();
        }

        public async Task<TicketSystem?> GetTicketByIdAsync(int id)
        {
            // Retrieve the ticket from the database
            var ticket = await _context.Tickets.FindAsync(id);

            // If the ticket is not found, return null
            if (ticket == null)
            {
                return null;
            }

            return ticket;
        }

        public async Task CreateTicketAsync(TicketSystem ticket)
        {
            // Validate the ticket
            ArgumentNullException.ThrowIfNull(ticket);

            if (string.IsNullOrEmpty(ticket.Title))
                throw new ArgumentException("Title is required");

            // Set default values
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            // Add to context and save
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
