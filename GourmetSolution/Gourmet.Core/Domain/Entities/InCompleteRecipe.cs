﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Domain.Entities
{
    //public class InCompleteRecipe
    //{
    //    [Key]
    //    public Guid Id { get; set; }
    //    [Required]
    //    public string FoodString { get; set; }
    //    [Required]
    //    public string Name { get; set; }
    //    [Required]
    //    public string ChefId { get; set; }
    //    [ForeignKey("ChefId")]
    //    public ApplicationUser chef { get; set; }
    //    [Required(ErrorMessage = "Description is required")]
    //    [MaxLength(15000)]
    //    public string Description { get; set; }
    //    //[Required]
    //    [ValidateNever]
    //    public string? ImgeUrl1 { get; set; }
    //    //[Required]
    //    [ValidateNever]
    //    public string? ImgeUrl2 { get; set; }
    //    //[Required]
    //    [ValidateNever]
    //    public string? ImgeUrl3 { get; set; }
    //    //[Required]
    //    [ValidateNever]
    //    public string? ImgeUrl4 { get; set; }
    //    //[Required]
    //    [ValidateNever]
    //    public string? ImgeUrl5 { get; set; }
    //    [Required]
    //    public string IngredientsString { get; set; }
    //    public string StepsString { get; set; }
    //    public string NotExistIngredients { get; set;}
    //    [Required]
    //    public Guid Primary_Source_of_IngredientId { get; set; }
    //    [ForeignKey("Primary_Source_of_IngredientId")]
    //    public Primary_Source_of_Ingredient primary_source_of_ingredient { get; set; }
    //    [Required]
    //    public Guid Cooking_MethodId { get; set; }
    //    [ForeignKey("Cooking_MethodId")]
    //    public Cooking_Method cooking_method { get; set; }
    //    [Required]
    //    public Guid Food_typeId { get; set; }
    //    [ForeignKey("Food_typeId")]
    //    public Food_type food_type { get; set; }
    //    [Required]
    //    public Guid NationalityId { get; set; }
    //    [ForeignKey("NationalityId")]
    //    public Nationality nationality { get; set; }
    //    [Required]
    //    public Guid Meal_TypeId { get; set; }
    //    [ForeignKey("Meal_Type")]
    //    public Meal_Type meal_type { get; set; }
    //    [Required]
    //    public Guid Difficulty_LevelId { get; set; }
    //    [ForeignKey("Difficulty_LevelId")]
    //    public Difficulty_Level difficulty_Level { get; set; }
    //    [Required]
    //    public int Time { get; set; }
    //    [Required]
    //    public int NumberOfPicture { get; set; }

    //}
}
