﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class ReadUserResponse
    {
        public string FullName {  get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Aboutme { get; set; }
        public string Gen { get; set; }
        public string ImageUrl { get; set; }
    }
}
