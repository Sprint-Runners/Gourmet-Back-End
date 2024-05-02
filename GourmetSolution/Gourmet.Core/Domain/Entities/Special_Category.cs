using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Entities
{
    public class Special_Category
    {
        [Key]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
