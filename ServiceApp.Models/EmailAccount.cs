namespace ServiceApp.Models;

public class EmailAccount : IProduct
{
    public string Email { get; set; }
    public string DisplayName { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }

    public string FriendlyName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(DisplayName))
                return Email + " (" + DisplayName + ")";
            return Email;
        }
    }

}
