using ServiceApp.API.Factories;
using ServiceApp.Models;
using System.Net;
using System.Net.Mail;

namespace ServiceApp.API.Services
{
    public class EmailService : IEmailService
    {
        public EmailAccount _emailAccount { get; }

        public EmailService(IFactory emailAccountFactory)
        {
            _emailAccount = (EmailAccount)emailAccountFactory.Create();
        }


        public virtual void SendEmail(
            string subject, 
            string emailBody,
            string receiverAddress, 
            string receiverName,
            string replyTo = null,
            string replyToName = null,
            IEnumerable<string> bcc = null, 
            IEnumerable<string> cc = null,
            string attachmentFilePath = null,
            string attachmentFileName = null,
            int attachedDownloadId = 0)
        {
            var message = new MailMessage();
            //from, to, reply to
            message.From = new MailAddress(_emailAccount.Email, "SERVICE");
            message.To.Add(new MailAddress(receiverAddress, receiverName));
            if (!String.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            //BCC
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !String.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            //CC
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !String.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            //content
            message.Subject = subject;
            message.Body = emailBody;
            message.IsBodyHtml = true;

            //create  the file attachment for this e-mail message
            if (!String.IsNullOrEmpty(attachmentFilePath) &&
                File.Exists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(attachmentFilePath);
                if (!String.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }
                message.Attachments.Add(attachment);
            }

            //send email
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = _emailAccount.UseDefaultCredentials;
                smtpClient.Host = _emailAccount.Host;
                smtpClient.Port = _emailAccount.Port;
                smtpClient.EnableSsl = _emailAccount.EnableSsl;
                if (_emailAccount.UseDefaultCredentials)
                    smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
                else
                    smtpClient.Credentials = new NetworkCredential(_emailAccount.Username, _emailAccount.Password);
                smtpClient.Send(message);
            }
        }
    }
}
