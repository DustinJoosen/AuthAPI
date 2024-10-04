using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message)
        {
            
        }
    }
}

