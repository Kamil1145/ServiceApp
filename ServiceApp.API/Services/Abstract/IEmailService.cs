namespace ServiceApp.API.Services
{
    public interface IEmailService
    {
        void SendEmail(string subject, string emailBody, string receiverAddress, string receiverName,
            string replyToAddress = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            string attachmentFilePath = null, string attachmentFileName = null,
            int attachedDownloadId = 0);
    }    
}
