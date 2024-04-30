using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gourmet.Core.Domain.Relations;

namespace Gourmet.Core.DTO.Response
{
    public class RecipeInformationResponse
    {
        public string Name { get; set; }
        public string FoodName { get; set; }
        public string ChefName { get; set; }
        public string ChefUserName {  get; set; }
        public string ChefImageUrl {  get; set; }
        public string Description { get; set; }
        public double Score { get; set; }
        public string ImgeUrl1 { get; set; }
        public string ImgeUrl2 { get; set; }
        public string ImgeUrl3 { get; set; }
        public string ImgeUrl4 { get; set; }
        public string ImgeUrl5 { get; set; }
        public List<Ingredient> List_Ingriedents { get; set; }
        public string PSOIName { get; set; }
        public string CMName { get; set; }
        public string FTName { get; set; }
        public string NName { get; set; }
        public string MTName { get; set; }
        public string DLName { get; set; }
        public int time {  get; set; }
        public List<RecipeStep> Steps { get; set; }
    }
}
