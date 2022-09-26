using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Models.Entities;
public class Ticket
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 2)]
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DueTime { get; set; }
    public User? ResponsibleUser { get; set; }
    public User? CreatedBy { get; set; }
    public User? ModifiedBy { get; set; }
    public string JiraTicketId { get; set; } = string.Empty;
    public TicketStatus TicketStatus { get; set; }
    public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    public TicketPriority Priority { get; set; }
}

