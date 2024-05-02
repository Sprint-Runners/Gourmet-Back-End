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
    public class Food
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [ValidateNever]
        public string ImgeUrl { get; set; }
        public int Timetocook { get; set; }
        public bool Special_Occasion { get; set; }
        public bool Is_Main { get; set; }
        public bool Is_breakfast { get; set; }

    }
}
