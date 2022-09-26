using ServiceApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceApp.Models.DTO;


public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public ICollection<RoleDto> Roles { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<TicketDto> TicketsCreated { get; set; } = new List<TicketDto>();
    public JiraCredentialsDto? JiraCredentials { get; set; }

}