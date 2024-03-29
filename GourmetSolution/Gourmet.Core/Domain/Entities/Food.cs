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
        //[Required]
        //public Guid Primary_Source_of_IngredientId { get; set; }
        //[ForeignKey("Primary_Source_of_IngredientId")]
        //public Primary_Source_of_Ingredient primary_source_of_ingredient { get; set; }
        //[Required]
        //public Guid Cooking_MethodId { get; set; }
        //[ForeignKey("Cooking_MethodId")]
        //public Cooking_Method cooking_method{ get; set; }
        //[Required]
        //public Guid Food_typeId { get; set; }
        //[ForeignKey("Food_typeId")]
        //public Food_type food_type{ get; set; }
        //[Required]
        //public Guid NationalityId { get; set; }
        //[ForeignKey("NationalityId")]
        //public Nationality nationality { get; set; }
        //[Required]
        //public Guid  Meal_TypeId { get; set; }
        //[ForeignKey("Meal_Type")]
        //public Meal_Type meal_type{ get; set; }
    }
}
