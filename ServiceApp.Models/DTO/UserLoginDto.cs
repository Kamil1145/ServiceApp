using System.Runtime.Serialization;

namespace ServiceApp.Models.DTO;


public class UserLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

