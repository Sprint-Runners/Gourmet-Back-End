using System;
using Gourmet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Gourmet.Core.DTO.Request
{
    public class SignUpRequest
    {
        [EmailAddress(ErrorMessage = "Email is not valid")]
        [Required(ErrorMessage = "{0} cannot be blank")]
        public string? Email { get; set; }
        //public string? UserName { get; set; }
        [Required(ErrorMessage = "{0} cannot be blank")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "" +
    "Your password is not strong")]
        public string? Password { get; set; }

        public User ToUser()
        {
            return new User() { Email = Email, 
                Password = Password,Id=Guid.NewGuid() };
        }
        
    }
}
