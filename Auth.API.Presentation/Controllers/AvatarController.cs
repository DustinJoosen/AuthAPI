using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Exceptions;
using Auth.API.Presentation.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.API.Presentation.Controllers
{
    [Route("avatar")]
    [ApiController]
    public class AvatarController : ControllerBase
    {

        private readonly IAvatarService _avatarService;
        public AvatarController(IAvatarService avatarService)
        {
            this._avatarService = avatarService;
        }

        [HttpPost]
        [Route("")]
        [JwtAuthorize]
        [LogWhenRequestMade]
        public async Task<IActionResult> SetAvatar([FromForm]IFormFile formFile)
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedException("JWT claim invalid");

            // TODO: FOrmFile Validation!!!!
            var uri = await this._avatarService.Upload(formFile.OpenReadStream(), Guid.Parse(userId));
            return Ok(uri);
        }

        [HttpDelete]
        [Route("")]
        [JwtAuthorize]
        [LogWhenRequestMade]
        public async Task<IActionResult> DeleteAvatar()
        {
            var userId = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedException("JWT claim invalid");

            await this._avatarService.Delete(Guid.Parse(userId));
            return Ok();
        }
    }
}
