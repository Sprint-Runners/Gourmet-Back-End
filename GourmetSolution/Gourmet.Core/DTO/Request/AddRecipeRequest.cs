using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Gourmet.Core.DTO.Request
{
    public class AddRecipeRequest
    {
        public string FoodName { get; set; }
        public string NotExistFoodName { get; set; }
        public string Description { get; set; }
        public List<Tuple<string, double>> List_Ingriedents { get; set; }
        public List<Tuple<string, double,string>> Not_Exist_List_Ingriedents { get; set; }
        public string primary_source_of_ingredient { get; set; }
        public string cooking_method { get; set; }
        public string food_type { get; set; }
        public string nationality { get; set; }
        public string meal_type { get; set; }
        public List<Tuple<int, string>> Steps { get; set; }
        public int Time {  get; set; }
        public string difficulty_level {  get; set; }
    }
    public class AddRecipeByAdminRequest
    {
        public string FoodName { get; set; }
        public string Description { get; set; }
        public string ChefUserID { get; set; }
        public List<Tuple<string, double>> List_Ingriedents { get; set; }
        public string primary_source_of_ingredient { get; set; }
        public string cooking_method { get; set; }
        public string food_type { get; set; }
        public string nationality { get; set; }
        public string meal_type { get; set; }
        public List<Tuple<int, string>> Steps { get; set; }
        public int Time { get; set; }
        public string difficulty_level { get; set; }
    }
}
