using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Entities
{
    public class User : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        public string? AvatarUri { get; set; }

        [Required]
        public bool EmailVerified { get; set; }

        public bool? Activated;
    }
}
