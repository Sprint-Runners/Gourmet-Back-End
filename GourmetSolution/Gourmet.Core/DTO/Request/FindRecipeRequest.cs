using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Request
{
    public class FindRecipeRequest
    {
        public string FoodName { get; set; }
        public string RecipeName { get; set; }
    }
}
