using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceApp.Models.Entities;
public class User
{
    [Required] 
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Phone]
    public string? PhoneNumber { get; set; } = string.Empty;
    public byte[]? PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public bool IsActive { get; set; } = false;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public string? ResetPasswordToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenCreated { get; set; }
    public DateTime RefreshTokenExpires { get; set; }

    [InverseProperty("ResponsibleUser")]
    public ICollection<Ticket> TicketsToProcess { get; set; } = new List<Ticket>();

    [InverseProperty("CreatedBy")]
    public ICollection<Ticket> TicketsCreated { get; set; } = new List<Ticket>();

    public JiraCredentials? JiraCredentials { get; set; }
}

