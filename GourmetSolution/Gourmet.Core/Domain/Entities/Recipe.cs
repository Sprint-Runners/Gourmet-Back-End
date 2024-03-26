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
    public class Recipe
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid FoodId { get; set; }
        [ForeignKey("FoodId")]
        public Food food { get; set; }
        [Required]
        public string ChefId { get; set; }
        [ForeignKey("ChefId")]
        public ApplicationUser chef { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(15000)]
        public string Description { get; set; }
        [Required]
        [Range(0,5)]
        public double Score {  get; set; }
        [Required]
        [ValidateNever]
        public string ImgeUrl { get; set; }
        [Required]
        public string List_Ingriedents { get; set; }


    }
}
