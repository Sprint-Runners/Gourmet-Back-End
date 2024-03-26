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
        public String RecentFoods { get; set; }
        public String FavoritFoods { get; set; }
    }
}