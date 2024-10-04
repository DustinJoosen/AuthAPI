using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Entities;
using Auth.API.Core.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        private readonly IVerificationTokenService _tokenService;

        protected string _bodyTemplate = @"
            <h1>Hello {username}!</h1>
            <p>
                You have requested your password to be reset.<br/>
                Please click the button below to reset it.
            </p>
            <button>
                <a href='google.com?q={token}'>Reset your password</a>
            </button>
            <p>
                Have a great day!<br/>
                -The Auth team
            </p>
        ";

        public PasswordResetService(IMailService mailService, IUserService userService,
            IVerificationTokenService verificationTokenService)
        {
            this._mailService = mailService;
            this._userService = userService;
            this._tokenService = verificationTokenService;
        }

        /// <summary>
        /// Generates a token and sends an email to send the user to the password reset page.
        /// </summary>
        /// <param name="userId">Id of the user to send the email to</param>
        /// <returns>Generated token</returns>
        public async Task<VerificationToken> Send(Guid userId)
        {
            var user = await this._userService.FindUser(userId);
            var token = await this._tokenService.CreateToken(user.Id);

            string body = this._bodyTemplate
                .Replace("{username}", user.UserName)
                .Replace("{token}", token.Token.ToString());

            this._mailService.SendEmail(user.Email, "Reset your password!", body);
            return token;
        }

        /// <summary>
        /// Handles updating of the password.
        /// </summary>
        /// <param name="token">Token to verify the email</param>
        /// <param name="resetPasswordReq">Password to update</param>
        /// <returns>Boolean determining success</returns>
        public async Task<bool> Reset(Guid token, ResetPasswordReq resetPasswordReq)
        {
            // Ensure token is valid. if not, exceptions are thrown.
            await this._tokenService.TryValidateToken(token);

            var tokenEntity = await this._tokenService.FindToken(token);

            await this._userService.UpdatePassword(tokenEntity.UserId, resetPasswordReq);
            await this._tokenService.DeleteToken(token);

            return true;
        }

    }
}
