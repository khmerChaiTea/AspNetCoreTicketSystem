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
                "Complete"
            });
        }

        // Calculate the number of days since creation
        public int DaysSinceCreation => (DateTime.UtcNow - CreatedAt).Days;

        // Calculate the time to complete, if completed
        public string TimeToComplete
        {
            get
            {
                if (Status == "Done" && CompletedAt.HasValue)
                {
                    int daysToComplete = (int)Math.Round((CompletedAt.Value - CreatedAt).TotalDays);
                    return $"{daysToComplete} days";
                }
                return "Ongoing";
            }
        }

        // Add this property
        public SelectList StatusOptions { get; set; }
    }
}
