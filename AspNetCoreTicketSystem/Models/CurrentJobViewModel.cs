using System;

namespace AspNetCoreTicketSystem.Models
{
    public class CurrentJobViewModel
    {
        public int Id { get; set; }  // Add this line
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Constructor to initialize properties
        public CurrentJobViewModel()
        {
            Name = string.Empty; // Initialize with a default value
            Status = string.Empty;
            Description = string.Empty;
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
                    // Calculate the time to complete in whole days
                    int daysToComplete = (int)Math.Round((CompletedAt.Value - CreatedAt).TotalDays);
                    return $"{daysToComplete} days";
                }
                return "Ongoing";
            }
        }
    }
}
