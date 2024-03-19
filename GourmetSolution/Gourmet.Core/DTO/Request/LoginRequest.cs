using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Request
{

        public class Login_Request
        {
            [EmailAddress(ErrorMessage = "Email is not valid")]
            [Required(ErrorMessage = "{0} cannot be blank")]
            public string? Email { get; set; }
            [Required(ErrorMessage = "{0} cannot be blank")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "" +
        "Your password is not strong")]
            public string? Password { get; set; }

        }
    
}
