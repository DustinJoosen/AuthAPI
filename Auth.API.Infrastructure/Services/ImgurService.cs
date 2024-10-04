using Auth.API.Abstractions.Interfaces;
using Auth.API.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Auth.API.Infrastructure.Services
{
    public class ImgurService : IImageService
    {
        private IConfiguration _configuration;
        public ImgurService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Uploads a stream to imgur.
        /// </summary>
        /// <param name="stream">Filestream to upload</param>
        /// <returns>Uri of upload image</returns>
        public async Task<string?> Upload(Stream stream)
        {
            string clientId = this._configuration["ImgurClientId"] ?? throw new KeyNotFoundException("Could not find imgur client id");

            using (HttpClient client = new HttpClient())
            {
                // Set headers.
                client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {clientId}");

                // Set multipart form content with image data.
                MultipartFormDataContent formData = new MultipartFormDataContent
                {
                    { new StreamContent(stream), "image", "image.jpg" }
                };

                var response = await client.PostAsync("https://api.imgur.com/3/image", formData);
                if (!response.IsSuccessStatusCode)
                    throw new ExternalServiceException("Could not upload image to external service imgur");

                // Get the url from the response.
                var content = await response.Content.ReadAsStringAsync();
                return this.FilterUrlOut(content);
            }
        }

        private string? FilterUrlOut(string json)
        {
            string pattern = "\"link\":\"([^\"]+)\"";

            Regex regex = new Regex(pattern);
            Match match = regex.Match(json);

            return match.Success
                ? match.Groups[1].Value
                : null;
        }
    }
}
