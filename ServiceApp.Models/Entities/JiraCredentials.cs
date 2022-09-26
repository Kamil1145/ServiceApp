namespace ServiceApp.Models.Entities
{
    public class JiraCredentials
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
