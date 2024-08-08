using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTicketSystem.Data;
using AspNetCoreTicketSystem.Models;
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

        public async Task<TicketSystem[]> GetAllTicketsAsync()
        {
            return await _context.Tickets.ToArrayAsync();
        }

        public async Task<TicketSystem> GetTicketByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                // Handle the null case appropriately
                throw new InvalidOperationException("Ticket not found");
                // Or provide a default instance
                // return new TicketSystem();
            }

            return ticket;
        }

        public async Task CreateTicketAsync(TicketSystem ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketAsync(TicketSystem ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
