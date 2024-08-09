using Microsoft.AspNetCore.Mvc;
using AspNetCoreTicketSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTicketSystem.Data; // Assuming you have ApplicationDbContext

public class CurrentJobsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CurrentJobsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var jobs = await _context.Tickets // Replace with your actual data source
            .Select(ticket => new CurrentJobViewModel
            {
                Name = ticket.Title,
                Status = ticket.Status,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                CompletedAt = ticket.CompletedAt
            })
            .ToListAsync();

        return View(jobs);
    }
}