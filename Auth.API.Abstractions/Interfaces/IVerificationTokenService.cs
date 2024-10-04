using Auth.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IVerificationTokenService
    {
        /// <summary>
        /// Attempts to validate a token based on existence and expiration date.<br/>
        /// if invalid, A BadRequestException is thrown.
        /// </summary>
        /// <param name="token">Token to validate</param>
        Task TryValidateToken(Guid token);
       
        /// <summary>
        /// Deletes a token.
        /// </summary>
        /// <param name="token">Token to delete</param>
        /// <returns>Boolean determining success</returns>
        Task<bool> DeleteToken(Guid token);

        /// <summary>
        /// Creates a unique token.
        /// </summary>
        /// <param name="userId">Id of the user to create the token for.</param>
        /// <returns>Created token</returns>
        Task<VerificationToken> CreateToken(Guid userId);

        /// <summary>
        /// Gives the entire token object from the token itself.
        /// </summary>
        /// <param name="token">Token to find the object from</param>
        /// <returns>Found token</returns>
        Task<VerificationToken> FindToken(Guid token);
    }
}
