using System;
using Gourmet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Request
{
    public class SignUpRequest
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public User ToUser()
        {
            return new User() { Email = Email, UserName = UserName,
                Password = Password,Id=Guid.NewGuid() };
        }
        
    }
}
