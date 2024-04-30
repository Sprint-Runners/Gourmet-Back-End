using Gourmet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SummaryRecipeInfoResponse
    {
        public string Name { get; set; }
        public string ChefName { get; set; }
        public string Description { get; set; }
        public string FoodName {  get; set; }
        public int Time {  get; set; }
        public String DifficultyLevel { get; set; }
        public string ChefUserName { get; set; }
        public double Score { get; set; }
        public string ImagePath { get; set; }
    }
}
