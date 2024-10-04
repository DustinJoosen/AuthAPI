using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Exchange
{
    public class LoginReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
