using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string? AvatarUri { get; set; }

        public bool EmailVerified { get; set; }
        public bool ResetRequested { get; set; }
    }
}
