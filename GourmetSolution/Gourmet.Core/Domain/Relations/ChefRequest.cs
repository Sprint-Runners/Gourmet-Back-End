using Gourmet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Relations
{
    public class ChefRequest
    {
        [Key]
        [Required]
        public string userId { get; set; }
        [Required]
        public string UserName{ get; set; }
        [ForeignKey("userId")]
        public ApplicationUser user { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
