using Microsoft.AspNetCore.Mvc;
using AspNetCoreTicketSystem.Data;
using AspNetCoreTicketSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace AspNetCoreTicketSystem.Controllers
{
	public class TicketsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public TicketsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Tickets
		public async Task<IActionResult> Index()
		{
			var tickets = await _context.Tickets.ToListAsync();
			return View(tickets);
		}

		// GET: Tickets/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var ticket = await _context.Tickets
				.FirstOrDefaultAsync(m => m.Id == id);
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
		public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,CreatedAt,UpdatedAt")] TicketSystem ticket)
		{
			if (ModelState.IsValid)
			{
				ticket.CreatedAt = DateTime.UtcNow;
				ticket.UpdatedAt = DateTime.UtcNow;
				_context.Add(ticket);
				await _context.SaveChangesAsync();
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

			var ticket = await _context.Tickets.FindAsync(id);
			if (ticket == null)
			{
				return NotFound();
			}
			return View(ticket);
		}

		// POST: Tickets/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,CreatedAt,UpdatedAt")] TicketSystem ticket)
		{
			if (id != ticket.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					ticket.UpdatedAt = DateTime.UtcNow;
					_context.Update(ticket);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TicketExists(ticket.Id))
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

			var ticket = await _context.Tickets
				.FirstOrDefaultAsync(m => m.Id == id);
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
			var ticket = await _context.Tickets.FindAsync(id);
			if (ticket == null)
			{
				// Handle the case when the ticket is not found, perhaps by redirecting to an error page or showing a message.
				return NotFound();
			}

			_context.Tickets.Remove(ticket);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool TicketExists(int id)
		{
			return _context.Tickets.Any(e => e.Id == id);
		}
	}
}
