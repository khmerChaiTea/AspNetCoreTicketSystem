using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreTicketSystem.Models;
using AspNetCoreTicketSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTicketSystem; // Adjust this if your Constants class is in a different namespace

[Authorize]
public class CurrentJobsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CurrentJobsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CurrentJobs
    public async Task<IActionResult> Index(string sortBy, string sortOrder)
    {
        // Fetch all non-deleted jobs
        var jobsQuery = _context.Tickets
            .Where(ticket => !ticket.IsDeleted)
            .Select(ticket => new CurrentJobViewModel
            {
                Id = ticket.Id,
                Name = ticket.Title,
                Status = ticket.Status,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt,
                CompletedAt = ticket.CompletedAt
            });

        // Apply sorting based on the column header clicked
        switch (sortBy)
        {
            case "Name":
                jobsQuery = sortOrder == "desc" ? jobsQuery.OrderByDescending(j => j.Name) : jobsQuery.OrderBy(j => j.Name);
                break;
            case "Status":
                jobsQuery = sortOrder == "desc" ? jobsQuery.OrderByDescending(j => j.Status) : jobsQuery.OrderBy(j => j.Status);
                break;
            case "Description":
                jobsQuery = sortOrder == "desc" ? jobsQuery.OrderByDescending(j => j.Description) : jobsQuery.OrderBy(j => j.Description);
                break;
            case "CreatedAt":
                jobsQuery = sortOrder == "desc" ? jobsQuery.OrderByDescending(j => j.CreatedAt) : jobsQuery.OrderBy(j => j.CreatedAt);
                break;
            default:
                jobsQuery = jobsQuery.OrderBy(j => j.Name);
                break;
        }

        // Execute the query and convert to a list
        var jobs = await jobsQuery.ToListAsync();

        // Pass sortBy and sortOrder to the View for sorting links
        ViewData["SortBy"] = sortBy;
        ViewData["SortOrder"] = sortOrder;

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
                new { Value = "Completed", Text = "Completed" }
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
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

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
        return _context.Tickets.Any(e => e.Id == id && !e.IsDeleted);
    }
}

