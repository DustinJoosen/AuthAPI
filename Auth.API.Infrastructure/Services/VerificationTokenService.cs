using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Entities;
using Auth.API.Core.Exceptions;
using Auth.API.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Services
{
    public class VerificationTokenService : IVerificationTokenService
    {
        private readonly AuthDbContext _context;
        public VerificationTokenService(AuthDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Creates a unique token.
        /// </summary>
        /// <param name="userId">Id of the user to create the token for.</param>
        /// <returns>Created token</returns>
        public async Task<VerificationToken> CreateToken(Guid userId)
        {
            var token = new VerificationToken
            {
                Token = Guid.NewGuid(),
                UserId = userId
            };

            this._context.VerificationTokens.Add(token);
            await this._context.SaveChangesAsync();

            return token!;
        }

        /// <summary>
        /// Deletes a token.
        /// </summary>
        /// <param name="token">Token to delete</param>
        /// <returns>Boolean determining success</returns>
        public async Task<bool> DeleteToken(Guid token)
        {
            var entity = await this._context.VerificationTokens.SingleOrDefaultAsync(tok => tok.Token == token);
            if (entity == null)
            {
                return false;
            }

            this._context.VerificationTokens.Remove(entity);
            await this._context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Gives the entire token object from the token itself.
        /// </summary>
        /// <param name="token">Token to find the object from</param>
        /// <returns>Found token</returns>
        public async Task<VerificationToken> FindToken(Guid token)
        {
            var entity = await this._context.VerificationTokens.SingleOrDefaultAsync(tok => tok.Token == token);
            if (entity == null)
                throw new BadRequestException("Verification token could not be found");

            return entity;
        }

        /// <summary>
        /// Attempts to validate a token based on existence and expiration date.<br/>
        /// if invalid, A BadRequestException is thrown.
        /// </summary>
        /// <param name="token">Token to validate</param>
        public async Task TryValidateToken(Guid token)
        {
            var entity = await this.FindToken(token);

            var tokenExpired = !((DateTime.UtcNow - entity.CreatedOn) < TimeSpan.FromHours(24));
            if (tokenExpired)
                throw new BadRequestException("Verification token is expired");
        }
    }
}
