using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.Domain.Relations;
using Gourmet.Core.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<UserResponse> Edit(EditUserRequest request, string username);
        Task<UserResponse> Read(string username);
        Task<IEnumerable<FavouritRecipeUser>> FavouritRecipeByUser(string userId);
        Task<IEnumerable<FavouritRecipeUser>> FavouritRecipeByUserOrderByScore(string userId);
        Task<IEnumerable<RecentRecipeUser>> RecentRecipeByUser(string userId);
        Task<InterGeneralResponse> AddFavouritRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName);
        Task<InterGeneralResponse> AddRecentRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName);
        Task<InterGeneralResponse> AddScoreRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName, int rate);
        Task<InterGeneralResponse> DeleteScoreRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName, int rate);
    }
}
