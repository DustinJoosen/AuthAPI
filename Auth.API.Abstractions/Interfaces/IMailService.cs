using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Sends an email. HTML-friendly.
        /// </summary>
        /// <param name="recipient">Recipient of the email</param>
        /// <param name="subject">Subject line of the email</param>
        /// <param name="body">Body of the email</param>
        void SendEmail(string recipient, string subject, string body);
    }
}
