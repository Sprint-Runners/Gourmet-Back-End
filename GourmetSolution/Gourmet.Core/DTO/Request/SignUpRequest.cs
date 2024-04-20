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
        [Required(ErrorMessage = "UserName is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "The Authentication code is required")]
        public int Temp_Code { get; set; }

    }
}