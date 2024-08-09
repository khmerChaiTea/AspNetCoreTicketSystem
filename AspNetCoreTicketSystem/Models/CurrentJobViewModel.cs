using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace AspNetCoreTicketSystem.Models
{
    public class CurrentJobViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Constructor to initialize properties
        public CurrentJobViewModel()
        {
            Name = string.Empty;
            Status = string.Empty;
            Description = string.Empty;
            StatusOptions = new SelectList(new[] {
                "Pending",
                "Need More Info",
                "Waiting on Parts",
                "Completed"
            });
        }

        // Calculate the number of days since creation
        public int DaysSinceCreation => (DateTime.UtcNow - CreatedAt).Days;

        // Return "Completed" if the status is "Completed", otherwise return "Ongoing"
        public string CompletionStatus
        {
            get
            {
                if (Status == "Completed")
                {
                    return "Completed";
                }
                return "Ongoing";
            }
        }

        // Add this property
        public SelectList StatusOptions { get; set; }
    }
}
