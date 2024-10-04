using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Exchange;
using Auth.API.Presentation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Presentation.Controllers
{
    [Route("password-reset")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly IVerificationTokenService _verificationTokenService;

        public PasswordResetController(IUserService userService, 
            IPasswordResetService passwordResetService, IVerificationTokenService verificationTokenService)
        {
            this._userService = userService;
            this._passwordResetService = passwordResetService;
            this._verificationTokenService = verificationTokenService;
        }

        [LogWhenRequestMade]
        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendPasswordResetMail([FromQuery] string email)
        {
            var user = await this._userService.FindUser(email);
            var token = await this._passwordResetService.Send(user.Id);

            return Ok(token.Token);
        }

        [LogWhenRequestMade]
        [HttpPost]
        [Route("{token:guid}")]
        public async Task<IActionResult> ResetUserPassword([FromRoute] Guid token, [FromBody] ResetPasswordReq resetPasswordReq)
        {
            await this._passwordResetService.Reset(token, resetPasswordReq);
            return Ok();
        }

        [LogWhenRequestMade]
        [HttpGet]
        [Route("token-is-valid/{token:guid}")]
        public async Task<IActionResult> IsTokenValid([FromRoute] Guid token)
        {
            await this._verificationTokenService.TryValidateToken(token);
            return Ok(new
            {
                statusCode = 200,
                message = "Verification token is valid"
            });
        }
    }
}
