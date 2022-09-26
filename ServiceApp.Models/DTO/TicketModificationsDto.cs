using ServiceApp.Models.Entities;

namespace ServiceApp.Models.DTO;


public class TicketModificationsDto
    {
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? DueTime { get; set; }
        public TicketPriority Priority { get; set; }
    }

