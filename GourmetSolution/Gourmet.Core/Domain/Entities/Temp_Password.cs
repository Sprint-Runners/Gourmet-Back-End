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
    public class Temp_Password
    {
        [Key]
        public string Email { get; set; }
        public string? Password { get; set; }
    }
}