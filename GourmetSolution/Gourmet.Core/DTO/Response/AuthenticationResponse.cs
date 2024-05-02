using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class AuthenticationResponse
    {
        //public string? Username;
        public string? Email { get; set; }
        public string? JWT_Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Period { get; set; }
        public string Role { get; set; }
    }
}