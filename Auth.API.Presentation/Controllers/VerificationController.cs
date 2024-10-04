using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Entities;
using Auth.API.Core.Exchange;
using Auth.API.Presentation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Auth.API.Presentation.Controllers
{
    [Route("verify")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailVerificationService _emailVerificationService;

        public VerificationController(IEmailVerificationService emailVerificationService, 
            IUserService userService)
        {
            this._userService = userService;
            this._emailVerificationService = emailVerificationService;
        }

        [LogWhenRequestMade]
        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendVerificationEmail([FromQuery] string email)
        {
            var user = await this._userService.FindUser(email);
            var token = await this._emailVerificationService.Send(user.Id);
            
            return Ok(token.Token);
        }

        [LogWhenRequestMade]
        [HttpPost]
        [Route("{token:guid}")]
        public async Task<IActionResult> VerifyUserEmail([FromRoute] Guid token)
        {
            await this._emailVerificationService.Verify(token);
            return Ok();
        }

    }
}
