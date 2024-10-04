﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.API.Core.Exceptions
{
    public class AlreadyUsedException : Exception
    {
        public AlreadyUsedException(string message) : base(message)
        {
            
        }
    }
}
