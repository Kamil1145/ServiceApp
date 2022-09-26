using ServiceApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceApp.Models.DTO;


public class TicketDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? JiraTicketId { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public List<CommentDto>? Comments { get; set; } = new();
    public TicketStatus TicketStatus { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueTime { get; set; }
    public TicketPriority Priority { get; set; }
    public Guid ModifiedById { get; set; }
    public UserDto? ResponsibleUser { get; set; }
    public UserDto? CreatedBy { get; set; }
}

