namespace AspNetCoreTicketSystem.Models
{
    public class TicketSystem
    {


        public int Id { get; set; }
        public string Title { get; set; } = "Default Title";
        public string Description { get; set; } = "Default Description";
        public string Status { get; set; } = "Default Status";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
