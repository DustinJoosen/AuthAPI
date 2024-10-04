using Auth.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Abstractions.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token.
        /// </summary>
        /// <param name="user">User to create the token for.</param>
        /// <returns>Created token.</returns>
        string GenerateToken(User user);
    }
}
