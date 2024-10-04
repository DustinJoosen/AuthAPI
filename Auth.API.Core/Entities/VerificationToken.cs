using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Entities
{
    public class VerificationToken : AuditableEntity
    {
        [Key, Required]
        public Guid Token { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
