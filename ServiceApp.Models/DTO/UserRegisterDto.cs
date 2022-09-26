using System.ComponentModel.DataAnnotations;

namespace ServiceApp.Models.DTO;

public class UserRegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;

    public bool? IsActive { get; set; } = false;
    public string? PhoneNumber { get; set; } = string.Empty;

    public int RoleId { get; set; } = 0;

}

