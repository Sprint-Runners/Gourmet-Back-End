using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Entities
{
    public class Primary_Source_of_Ingredient
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        public string ImageUrl {  get; set; }
    }
    public class Cooking_Method
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
    public class Food_type
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
    public class Nationality
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
    public class Meal_Type
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        public string ImageUrl { get; set; }

    }
    public class Difficulty_Level
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
    }


}