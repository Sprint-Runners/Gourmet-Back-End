using Gourmet.Core.DTO.Request;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Entities
{
    public class Email_Pass
    {
        [Key]
        public string Email { get; set; }
        public int Temp_Password { get; set; }
    }
}