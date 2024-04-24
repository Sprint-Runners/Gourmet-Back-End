using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.Relations;
using Gourmet.Core.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IRecipeService
    {
        Task<RecipeResponse> CreateRecipeByChef(AddRecipeRequest request, string userId, string username);
        Task<InCompleteRecipeResponse> CreateInCompleteRecipe(AddRecipeRequest request, string userId, string username);
        Task<IEnumerable<Recipe>> Get_All_Recipe();
        Task<IEnumerable<Ingredient>> Get_All_Ingredients(string FoodName, string ChefName, string RecipeName);
        Task<IEnumerable<RecipeStep>> Get_All_steps(string FoodName, string ChefName, string RecipeName);
    }
}
