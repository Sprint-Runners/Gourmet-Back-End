﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException (string? message) : base(message)
        {

        }
    }
}
