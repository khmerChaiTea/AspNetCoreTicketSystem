using AspNetCoreTicketSystem.Models;

namespace AspNetCoreTicketSystem.ViewModels
{
	public class TicketViewModel
	{
		public TicketSystem[] Tickets { get; set; } = []; // Initialize with an empty array
		public IEnumerable<string> AvailableStatuses { get; set; } = []; // Initialize with an empty collection
	}
}
