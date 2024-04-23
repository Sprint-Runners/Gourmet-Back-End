using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Other_Object
{
    public class Response
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public Chef user { get; set; }
    }
    public class Email_Response
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }
}