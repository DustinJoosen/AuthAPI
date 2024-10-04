using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Entities;
using Auth.API.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly IVerificationTokenService _verificationTokenService;

        protected string _bodyTemplate = @"
            <h1>Hello {username}!</h1>
            <p>
                You have created an account with us.<br/>
                Please click the button below to confirm your email addres.
            </p>
            <button>
                <a href='http://localhost:4200/confirm-email?token={token}'>Confirm your email</a>
            </button>
            <p>You have 24 hours to confirm the email, before you need to resend this email.</p>
            <p>
                Have a great day!<br/>
                -The Auth team
            </p>
        ";

        public EmailVerificationService(IMailService mailService, IUserService userService,
            IVerificationTokenService verificationTokenService)
        {
            this._mailService = mailService;
            this._userService = userService;
            this._verificationTokenService = verificationTokenService;
        }

        /// <summary>
        /// Generates a token and sends an email to verify the email.
        /// </summary>
        /// <param name="userId">Id of the user to verify</param>
        /// <returns>Generated token</returns>
        public async Task<VerificationToken> Send(Guid userId)
        {
            var user = await this._userService.FindUser(userId);
            var token = await this._verificationTokenService.CreateToken(user.Id);

            string body = this._bodyTemplate
                .Replace("{username}", user.UserName)
                .Replace("{token}", token.Token.ToString());
            
            this._mailService.SendEmail(user.Email, "Verify your email!", body);
            return token;
        }

        /// <summary>
        /// Handles verification of the email.
        /// </summary>
        /// <param name="token">Token to verify the email</param>
        /// <returns>Boolean determining success</returns>
        public async Task<bool> Verify(Guid token)
        {
            // Ensure token is valid. if not, exceptions are thrown.
            await this._verificationTokenService.TryValidateToken(token);

            var entity = await this._verificationTokenService.FindToken(token);

            await this._userService.VerifyEmail(entity.UserId);
            await this._verificationTokenService.DeleteToken(token);

            return true;
        }
    }
}
