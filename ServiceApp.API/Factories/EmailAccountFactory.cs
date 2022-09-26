using ServiceApp.Models;

namespace ServiceApp.API.Factories
{
    public class EmailAccountFactory : IFactory
    {
        private readonly string Email;
        private readonly string DisplayName;
        private readonly string Host;
        private readonly string Username;
        private readonly string Password;
        private readonly int Port;
        private readonly bool EnableSsl;
        public EmailAccountFactory(IConfiguration configuration)
        {
            Email = configuration.GetValue(typeof(string), "AppSettings:Email").ToString();
            DisplayName = configuration.GetValue(typeof(string), "AppSettings:DisplayName").ToString();
            Host = configuration.GetValue(typeof(string), "AppSettings:Host").ToString();
            Username = configuration.GetValue(typeof(string), "AppSettings:Username").ToString();
            Password = configuration.GetValue(typeof(string),"AppSettings:Password").ToString();
            Port = (int)configuration.GetValue(typeof(int),"AppSettings:Port");
            EnableSsl = (bool)configuration.GetValue(typeof(bool),"AppSettings:EnableSsl");
        }


        public IProduct Create()
        {
            return new EmailAccount()
            {
                Email = Email,
                DisplayName = DisplayName,
                Host = Host,
                Username = Username,
                Password = Password,
                Port = Port,
                EnableSsl = EnableSsl
            };
        }
    }
}
