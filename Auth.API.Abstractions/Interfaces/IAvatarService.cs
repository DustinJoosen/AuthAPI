using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IAvatarService
    {
        /// <summary>
        /// Sets the avatar of a user to null, effectively deleting the avatar.
        /// </summary>
        /// <param name="userId">User to remove the avatar from.</param>
        Task Delete(Guid userId);

        /// <summary>
        /// Uploads an image, and assign it to the user as an avatar.
        /// </summary>
        /// <param name="stream">Stream of the image to upload.</param>
        /// <param name="userId">User to set the avatar for.</param>
        /// <returns>URI of uploaded image.</returns>
        Task<string> Upload(Stream stream, Guid userId);
    }
}
