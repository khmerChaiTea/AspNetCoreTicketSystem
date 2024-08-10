using Microsoft.AspNetCore.Mvc;
using AspNetCoreTicketSystem.Data;
using AspNetCoreTicketSystem.Models;
using AspNetCoreTicketSystem.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTicketSystem.Controllers
{
    [Authorize]
    public class TicketsController(ITicketSystemService ticketService, UserManager<IdentityUser> userManager) : Controller
    {
        private readonly ITicketSystemService _ticketService = ticketService;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var tickets = await _ticketService.GetActiveTicketsAsync(currentUser);
            return View(tickets);
        }

        // GET: Tickets/History
        public async Task<IActionResult> History()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] TicketSystem ticket)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();

                ticket.CreatedAt = DateTime.UtcNow;
                ticket.UpdatedAt = DateTime.UtcNow;
                ticket.Status = "Created"; // Set default status
                ticket.UserId = currentUser.Id;

                await _ticketService.CreateTicketAsync(ticket);
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status")] TicketSystem ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var existingTicket = await _ticketService.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            var isHelpdesk = User.IsInRole(Constants.HelpdeskRole);

            if (!isHelpdesk)
            {
                ticket.Status = existingTicket.Status; // Preserve original status for non-helpdesk users
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingTicket.Title = ticket.Title;
                    existingTicket.Description = ticket.Description;
                    if (isHelpdesk) existingTicket.Status = ticket.Status; // Allow status update only for helpdesk users
                    existingTicket.UpdatedAt = DateTime.UtcNow;
                    existingTicket.UserId = currentUser.Id;

                    await _ticketService.UpdateTicketAsync(existingTicket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TicketExists(int id)
        {
            return await _ticketService.GetTicketByIdAsync(id) != null;
        }
    }
}
