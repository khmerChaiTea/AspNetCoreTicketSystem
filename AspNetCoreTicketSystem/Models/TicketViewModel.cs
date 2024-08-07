using AspNetCoreTicketSystem.Models;

namespace AspNetCoreTicketSystem.ViewModels
{
	public class TicketViewModel
	{
		public TicketSystem[] Tickets { get; set; } = Array.Empty<TicketSystem>(); // Initialize with an empty array
		public IEnumerable<string> AvailableStatuses { get; set; } = Enumerable.Empty<string>(); // Initialize with an empty collection
	}
}
