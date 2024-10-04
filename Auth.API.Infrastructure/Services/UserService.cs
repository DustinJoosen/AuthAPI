using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Entities;
using Auth.API.Core.Exceptions;
using Auth.API.Core.Exchange;
using Auth.API.Infrastructure.Utils;
using Auth.API.Persistence;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _context;
        public UserService(AuthDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Finds the user based on their Id.
        /// </summary>
        /// <param name="userId">Id to search the user from</param>
        /// <returns>Found user.</returns>
        public async Task<User> FindUser(Guid userId)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(user => user.Id == userId);
            if (user == null)
                throw new NotFoundException("Could not find user by userId");

            return user;
        }

        /// <summary>
        /// Finds the user based on their Email.
        /// </summary>
        /// <param name="email">Email to search the user from</param>
        /// <returns>Found user.</returns>
        public async Task<User> FindUser(string email)
        {
            var user = await this._context.Users.SingleOrDefaultAsync(user => user.Email == email);
            if (user == null)
                throw new NotFoundException("Could not find user by email");

            return user;
        }
        /// <summary>
        /// Sets the field "email verified" to true.
        /// </summary>
        /// <param name="userId">Id of the user to apply the verification to</param>
        public async Task VerifyEmail(Guid userId)
        {
            var user = await this.FindUser(userId);
            user.EmailVerified = true;

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="registerUserReq">User data to register</param>
        /// <returns>The registered user data</returns>
        public async Task<RegisterUserRes> RegisterUser(RegisterUserReq registerUserReq)
        {
            var registration = registerUserReq.Registration;

            #region validation

            if (this.EmailInUse(registration.Email))
                throw new AlreadyUsedException("This email is already in use");

            if (!this.EmailInCorrectFormat(registration.Email))
                throw new BadRequestException("The email field is not formatted properly");

            if (this.UsernameInUse(registration.UserName))
                throw new AlreadyUsedException("This username is already in use");

            if (!PasswordPolicy.IsPasswordValid(registration.Password, out string message))
                throw new BadRequestException(message);

            #endregion

            registration.Password = BCrypt.Net.BCrypt.HashPassword(registration.Password);

            var user = new User
            {
                UserName = registration.UserName,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Email = registration.Email,
                Password = registration.Password,
                AvatarUri = registration.AvatarUri,
                EmailVerified = false,
                Activated = true,
            };

            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();

            registration.Password = "********";
            return new RegisterUserRes { Data = registration };
        }

        /// <summary>
        /// Updates the password of the user, after a reset is requested.
        /// </summary>
        /// <param name="userId">User to update the password for.</param>
        /// <param name="resetPasswordReq">Password to update.</param>
        public async Task UpdatePassword(Guid userId, ResetPasswordReq resetPasswordReq)
        {
            if (!PasswordPolicy.IsPasswordValid(resetPasswordReq.UpdatedPassword, out string message))
                throw new BadRequestException(message);

            var user = await this.FindUser(userId);
            user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordReq.UpdatedPassword);

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the avatar of the user.
        /// </summary>
        /// <param name="userId">User to update the avatar for.</param>
        /// <param name="uri">Avatar to update.</param>
        public async Task UpdateAvatarUri(Guid userId, string uri)
        {
            var user = await this.FindUser(userId);
            user.AvatarUri = uri;

            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Attempts to validate a user login based on email and password.<br/>
        /// if invalid, A BadRequestException is thrown.
        /// </summary>
        /// <param name="loginReq">Credentials to check.</param>
        /// <returns>If valid, the validated user object.</returns>
        public async Task<User> TryUserLoginValid(LoginReq loginReq)
        {
            var user = await this.FindUser(loginReq.Email);

            if (user.Email.ToLower() != loginReq.Email.ToLower())
                throw new UnauthorizedException("Email is incorrect");

            if (!BCrypt.Net.BCrypt.Verify(loginReq.Password, user.Password))
                throw new UnauthorizedException("Password is incorrect");

            if (!user.EmailVerified)
                throw new UnauthorizedException("Email is not yet validated");

            return user;
        }

        /// <summary>
        /// PERNAMENTLY Deactivates a user. The data is kept, but the user can't log in anymore.
        /// </summary>
        /// <param name="userId">User to deactivate.</param>
        public async Task Deactivate(Guid userId)
        {
            var user = await this.FindUser(userId);
            user.Activated = false;

            this._context.Entry(user).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }

        private bool EmailInUse(string email)
        {
            return this._context.Users.Any(user => user.Email == email);
        }

        private bool UsernameInUse(string username)
        {
            return this._context.Users.Any(user => user.UserName == username);
        }

        private bool EmailInCorrectFormat(string email)
        {
            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

    }
}
