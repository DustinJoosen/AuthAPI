using Auth.API.Core.Entities;
using Auth.API.Core.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="registerUserReq">User data to register</param>
        /// <returns>The registered user data</returns>
        Task<RegisterUserRes> RegisterUser(RegisterUserReq registerUserReq);

        /// <summary>
        /// Finds the user based on their Id.
        /// </summary>
        /// <param name="userId">Id to search the user from</param>
        /// <returns>Found user.</returns>
        Task<User> FindUser(Guid userId);

        /// <summary>
        /// Finds the user based on their Email.
        /// </summary>
        /// <param name="email">Email to search the user from</param>
        /// <returns>Found user.</returns>
        Task<User> FindUser(string email);

        /// <summary>
        /// Sets the field "email verified" to true.
        /// </summary>
        /// <param name="userId">Id of the user to apply the verification to</param>
        Task VerifyEmail(Guid userId);

        /// <summary>
        /// Updates the password of the user, after a reset is requested.
        /// </summary>
        /// <param name="userId">User to update the password for.</param>
        /// <param name="resetPasswordReq">Password to update.</param>
        Task UpdatePassword(Guid userId, ResetPasswordReq resetPasswordReq);

        /// <summary>
        /// Updates the avatar of the user.
        /// </summary>
        /// <param name="userId">User to update the avatar for.</param>
        /// <param name="uri">Avatar to update.</param>
        Task UpdateAvatarUri(Guid userId, string uri);

        /// <summary>
        /// Attempts to validate a user login based on email and password.<br/>
        /// if invalid, A BadRequestException is thrown.
        /// </summary>
        /// <param name="loginReq">Credentials to check.</param>
        /// <returns>If valid, the validated user object.</returns>
        Task<User> TryUserLoginValid(LoginReq loginReq);
        
        /// <summary>
        /// PERNAMENTLY Deactivates a user. The data is kept, but the user can't log in anymore.
        /// </summary>
        /// <param name="userId">User to deactivate.</param>
        Task Deactivate(Guid userId);
    }
}
