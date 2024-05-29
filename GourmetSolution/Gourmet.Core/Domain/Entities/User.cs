using Gourmet.Core.DTO.Request;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string Aboutme { get; set; }
        public string Gender { get; set; }
        public bool Ban { get; set; }=false;
        public DateTime premium { get; set; }= DateTime.Now.AddDays(1);

    }
}