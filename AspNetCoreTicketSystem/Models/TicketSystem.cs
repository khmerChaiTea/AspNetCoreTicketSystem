namespace AspNetCoreTicketSystem.Models
{
    public class TicketSystem
    {


        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; } // Add this property
        public bool IsDeleted { get; set; } = false; // New property
        public string UserId { get; set; } = string.Empty;
    }
}
