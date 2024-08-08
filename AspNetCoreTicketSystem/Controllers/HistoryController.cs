using Microsoft.AspNetCore.Mvc;
using AspNetCoreTicketSystem.Data;
using AspNetCoreTicketSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreTicketSystem.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                                         .OrderBy(t => t.CreatedAt)
                                         .ToListAsync();
            return View(tickets);
        }
    }
}
