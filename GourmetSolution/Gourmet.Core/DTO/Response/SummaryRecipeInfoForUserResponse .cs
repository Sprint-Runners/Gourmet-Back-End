using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SummaryRecipeInfoForUserResponse
    {
        public string Name {  get; set; }
        public string ChefName { get; set; }
        public string ChefUserName { get; set; }
        public double Score {  get; set; }
        public string FoodName { get; set; }
        public int Time { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime VisitTime { get; set; }
        public string PSOIName { get; set; }
        public string CMName { get; set; }
        public string FTName { get; set; }
        public string NName { get; set; }
        public string MTName { get; set; }
        public string DLName { get; set; }
    }
}
