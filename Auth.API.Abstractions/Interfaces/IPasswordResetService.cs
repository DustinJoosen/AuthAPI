using Auth.API.Core.Entities;
using Auth.API.Core.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IPasswordResetService
    {
        /// <summary>
        /// Generates a token and sends an email to send the user to the password reset page.
        /// </summary>
        /// <param name="userId">Id of the user to send the email to</param>
        /// <returns>Generated token</returns>
        Task<VerificationToken> Send(Guid userId);

        /// <summary>
        /// Handles updating of the password.
        /// </summary>
        /// <param name="token">Token to verify the email</param>
        /// <param name="resetPasswordReq">Password to update</param>
        /// <returns>Boolean determining success</returns>
        Task<bool> Reset(Guid token, ResetPasswordReq resetPasswordReq);
    }
}
