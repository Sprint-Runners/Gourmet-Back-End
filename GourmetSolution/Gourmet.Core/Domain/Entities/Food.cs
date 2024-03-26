using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Entities
{
    public class Food
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
    }
}
