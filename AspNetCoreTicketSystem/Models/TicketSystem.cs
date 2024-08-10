namespace AspNetCoreTicketSystem.Models
{
    public class TicketSystem
    {


        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; } // Add this property
        public bool IsDeleted { get; set; } = false; // New property
        public string? UserId { get; set; }
    }
}
