using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Relations
{
    public class RecipeIngredients
    {
        [Required]
        public Guid RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe recipe { get; set; }
        [Required]
        public Guid IngredientId { get; set; }
        [ForeignKey("IngredientId")]
        public Ingredient ingredient { get; set; }
        [Required]
        public double Quantity {  get; set; }


    }
}
