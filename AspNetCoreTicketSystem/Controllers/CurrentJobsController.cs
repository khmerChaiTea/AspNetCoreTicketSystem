using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreTicketSystem.Models;
using AspNetCoreTicketSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTicketSystem; // Adjust this if your Constants class is in a different namespace

[Authorize(Roles = Constants.HelpdeskRole)]
public class CurrentJobsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CurrentJobsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CurrentJobs
    public async Task<IActionResult> Index()
    {
        var jobs = await _context.Tickets
            .Where(ticket => !ticket.IsDeleted) // Exclude deleted tickets
            .Select(ticket => new CurrentJobViewModel
            {
                Id = ticket.Id, // Ensure Id is included in ViewModel
                Name = ticket.Title,
                Status = ticket.Status,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                CompletedAt = ticket.CompletedAt
            })
            .ToListAsync();

        return View(jobs);
    }

    // GET: CurrentJobs/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        if (ticket == null)
        {
            return NotFound();
        }

        var viewModel = new CurrentJobViewModel
        {
            Id = ticket.Id,
            Name = ticket.Title,
            Status = ticket.Status,
            Description = ticket.Description,
            CreatedAt = ticket.CreatedAt,
            CompletedAt = ticket.CompletedAt,
            StatusOptions = new SelectList(new[]
            {
            new { Value = "Pending", Text = "Pending" },
            new { Value = "Need More Info", Text = "Need More Info" },
            new { Value = "Waiting on Parts", Text = "Waiting on Parts" },
            new { Value = "Complete", Text = "Complete" }
        }, "Value", "Text", ticket.Status)
        };

        return View(viewModel);
    }

    // POST: CurrentJobs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CurrentJobViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted); // Check for deletion

            if (ticket == null)
            {
                return NotFound();
            }

            // Update ticket properties
            ticket.Status = model.Status;

            // Only set CompletedAt if status is "Complete", otherwise set to null
            ticket.CompletedAt = model.Status == "Complete" ? DateTime.UtcNow : (DateTime?)null;

            try
            {
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

        return View(model);
    }

    private bool TicketExists(int id)
    {
        return _context.Tickets.Any(e => e.Id == id && !e.IsDeleted); // Check for deletion
    }
}
