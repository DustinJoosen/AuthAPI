using Auth.API.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Auth.API.Core.Options;
using Microsoft.Extensions.Options;

namespace Auth.API.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly MailConfig _mailConfig;
        public MailService(IOptions<MailConfig> mailConfig)
        {
            this._mailConfig = mailConfig.Value;
        }

        /// <summary>
        /// Sends an email. HTML-friendly.
        /// </summary>
        /// <param name="recipient">Recipient of the email</param>
        /// <param name="subject">Subject line of the email</param>
        /// <param name="body">Body of the email</param>
        public void SendEmail(string recipient, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(this._mailConfig.MailAddress);
                mail.To.Add(recipient);

#if DEBUG
                // Don't want to accidentally spam random people. If in debug, override it.
                mail.To.Clear();
                mail.To.Add("dustinjoosen2003@gmail.com");
#endif

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(this._mailConfig.MailAddress, this._mailConfig.MailAppCode);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }

    }
}
