using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Request
{
    public class EditRecipeRequest
    {
        public Guid ID {  get; set; }
        public string FoodName { get; set; }
        public string NotExistFoodName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tuple<string, string, string, bool>> List_Ingriedents { get; set; }
        //public List<Tuple<string, double,string>> Not_Exist_List_Ingriedents { get; set; }
        public string primary_source_of_ingredient { get; set; }
        public string cooking_method { get; set; }
        public string food_type { get; set; }
        public string nationality { get; set; }
        public string meal_type { get; set; }
        public List<Tuple<string, string>> Steps { get; set; }
        public string Time { get; set; }
        public string difficulty_level { get; set; }
        public int NumberOfPicture { get; set; }
    }
}
