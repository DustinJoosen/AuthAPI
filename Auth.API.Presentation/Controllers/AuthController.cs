using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Exceptions;
using Auth.API.Core.Exchange;
using Auth.API.Presentation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.API.Presentation.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            this._userService = userService;
            this._jwtService = jwtService;
        }


        [LogWhenRequestMade]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq loginReq)
        {
            var user = await this._userService.TryUserLoginValid(loginReq);
            var token = this._jwtService.GenerateToken(user);

            return Ok(token);
        }

        [LogWhenRequestMade]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserReq registerUserReq)
        {
            var user = await this._userService.RegisterUser(registerUserReq);
            return Ok(user);
        }

        [HttpDelete]
        [Route("deactivate")]
        [JwtAuthorize]
        public async Task<IActionResult> Deactivate()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedException("JWT claim invalid");

            await this._userService.Deactivate(Guid.Parse(userId));
            return Ok();
        }

        [LogWhenRequestMade]
        [HttpGet]
        [Route("who-am-i")]
        [JwtAuthorize]
        public async Task<IActionResult> WhoAmI()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedException("JWT claim invalid");

            var user = await this._userService.FindUser(Guid.Parse(userId));
            if (user == null)
                throw new UnauthorizedException("UserId in JWT claim could not be found");

            user.Password = "********";
            return Ok(user);
        }

    }
}
