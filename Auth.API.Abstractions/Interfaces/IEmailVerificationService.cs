using Auth.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IEmailVerificationService
    {
        /// <summary>
        /// Generates a token and sends an email to verify the email.
        /// </summary>
        /// <param name="userId">Id of the user to verify</param>
        /// <returns>Generated token</returns>
        Task<VerificationToken> Send(Guid userId);

        /// <summary>
        /// Handles verification of the email.
        /// </summary>
        /// <param name="token">Token to verify the email</param>
        /// <returns>Boolean determining success</returns>
        Task<bool> Verify(Guid token);
    }
}
