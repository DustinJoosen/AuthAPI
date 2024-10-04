using Auth.API.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Exchange
{
    public class RegisterUserReq
    {
        public RegistrationDto Registration { get; set; }
    }
        
    public class RegisterUserRes
    {
        public RegistrationDto Data { get; set; }
    }
}
