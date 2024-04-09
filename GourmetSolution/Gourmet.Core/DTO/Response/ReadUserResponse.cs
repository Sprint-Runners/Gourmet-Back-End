using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class ReadUserResponse
    {
        public string FirstNmae {  get; set; }
        public string LastNmae { get; set; }
        public string UserNmae { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string aboutme { get; set; }
    }
}
