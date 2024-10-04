using Auth.API.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        public AvatarService(IUserService userService, IImageService imageService)
        {
            this._userService = userService;
            this._imageService = imageService;
        }

        /// <summary>
        /// Sets the avatar of a user to null, effectively deleting the avatar.
        /// </summary>
        /// <param name="userId">User to remove the avatar from.</param>
        public async Task Delete(Guid userId)
        {
            await this._userService.UpdateAvatarUri(userId, null);
        }

        /// <summary>
        /// Uploads an image, and assign it to the user as an avatar.
        /// </summary>
        /// <param name="stream">Stream of the image to upload.</param>
        /// <param name="userId">User to set the avatar for.</param>
        /// <returns>URI of uploaded image.</returns>
        public async Task<string> Upload(Stream stream, Guid userId)
        {
            var uri = await this._imageService.Upload(stream);
            await this._userService.UpdateAvatarUri(userId, uri);

            return uri;
        }
    }
}
