using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Uploads a stream to imgur.
        /// </summary>
        /// <param name="stream">Filestream to upload</param>
        /// <returns>Uri of upload image</returns>
        public Task<string?> Upload(Stream stream);
    }
}
