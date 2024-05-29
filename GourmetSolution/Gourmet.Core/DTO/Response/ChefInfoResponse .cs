using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class ChefInfoResponse
    {
        public double Score { get; set; }
        public int RecipeCount { get; set; }
        public List<SummaryRecipeInfoAddedByChefResponse> AcceptRecipes { get; set; }
        public List<SummaryRecipeInfoAddedByChefResponse> NotAcceptRecipes { get; set; }
    }
}
