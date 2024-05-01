using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.Domain.Relations;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace Gourmet.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Chef> _userManager;
        private readonly AppDbContext _db;
        private readonly IRecipeService _recipeService;
        public UserService(UserManager<Chef> userManager, AppDbContext db, IRecipeService recipeService)
        {
            _userManager = userManager;
            _db = db;
            _recipeService = recipeService;
        }
        public async Task<UserResponse> Edit(EditUserRequest request, string username)
        {
            var isExistsUser = await _userManager.FindByNameAsync(username);
            if (isExistsUser == null)
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = "UserName not Exists",
                    user = null
                };

            Chef EditUser = (Chef)isExistsUser;
            EditUser.Email = request.Email;
            EditUser.FullName = request.FullName;
            EditUser.PhoneNumber = request.PhoneNumber;
            EditUser.Aboutme = request.Aboutme;
            EditUser.Gender = request.Gen;
            //EditUser.UserName
            var result = await _userManager.UpdateAsync(EditUser);

            if (result.Succeeded)
                return new UserResponse()
                {
                    IsSucceed = true,
                    Message = "Update Successfully",
                    user = EditUser
                };
            var errorString = "User Updat Failed Beacause: ";
            foreach (var error in result.Errors)
            {
                errorString += " # " + error.Description;
            }
            return new UserResponse()
            {
                IsSucceed = false,
                Message = errorString,
                user = null
            };
        }
        public async Task<UserResponse> Read(string username)
        {
            var isExistsUser = await _userManager.FindByNameAsync(username);

            if (isExistsUser == null)
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = "UserName not Exists",
                    user = null
                };
            return new UserResponse()
            {
                IsSucceed = true,
                Message = "Read Successfully",
                user = isExistsUser
            };

        }
        public async Task<IEnumerable<FavouritRecipeUser>> FavouritRecipeByUser(string userId)
        {
            var Recipes=_db.FavouritRecipeUsers.Where(r => r.userId == userId).OrderByDescending(r => r.TimeToLike).ToList();
            return Recipes;
        }
        public async Task<IEnumerable<FavouritRecipeUser>> FavouritRecipeByUserOrderByScore(string userId)
        {
            var Recipes = _db.FavouritRecipeUsers.Where(r => r.userId == userId).OrderByDescending(r => r.recipe.Score).ToList();
            return Recipes;
        }
        public async Task<IEnumerable<RecentRecipeUser>> RecentRecipeByUser(string userId)
        {
            var Recipes =  _db.RecentRecipeUsers.Where(r => r.userId == userId).OrderByDescending(r => r.VisitTime).ToList();
            return Recipes;
        }
        public async Task<InterGeneralResponse> AddFavouritRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName)
        {

            try
            {
                var Recipe = await _recipeService.Search_Recipe(FoodName, ChefName, RecipeName);
                var IsExistFavouritRecipeUser = _db.FavouritRecipeUsers.Where(r => r.userId == user.Id && r.recipe.food.Name.ToLower() == FoodName.ToLower() && r.recipe.chef.UserName.ToLower() == ChefName.ToLower() && r.recipe.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
                if (IsExistFavouritRecipeUser == null)
                {
                    _db.FavouritRecipeUsers.Add(new FavouritRecipeUser
                    {
                        recipe = Recipe,
                        RecipeId = Recipe.Id,
                        TimeToLike = DateTime.Now,
                        userId = user.Id,
                        user = user
                    });
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Add Favourit Successfully"
                    };
                }
                else
                {
                    _db.FavouritRecipeUsers.Remove(IsExistFavouritRecipeUser);
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Delete Favourit Successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = ex.Message
                };
            }

        }
        public async Task<InterGeneralResponse> AddRecentRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName)
        {
            try
            {
                var Recipe = await _recipeService.Search_Recipe(FoodName, ChefName, RecipeName);
                var IsExistRecentRecipeUser = _db.RecentRecipeUsers.Where(r => r.userId == user.Id && r.recipe.food.Name.ToLower() == FoodName.ToLower() && r.recipe.chef.UserName.ToLower() == ChefName.ToLower() && r.recipe.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
                if (IsExistRecentRecipeUser == null)
                {
                    _db.RecentRecipeUsers.Add(new RecentRecipeUser
                    {
                        recipe = Recipe,
                        RecipeId = Recipe.Id,
                        VisitTime = DateTime.Now,
                        userId = user.Id,
                        user = user
                    });
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Add Recent Successfully"
                    };
                }
                else
                {
                    IsExistRecentRecipeUser.VisitTime = DateTime.Now;
                    _db.RecentRecipeUsers.Update(IsExistRecentRecipeUser);
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Update Recent Successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<InterGeneralResponse> AddScoreRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName,int rate)
        {
            try
            {
                var Recipe = await _recipeService.Search_Recipe(FoodName, ChefName, RecipeName);
                var IsExistScoreRecipeUser = _db.ScoreRecipeUsers.Where(r => r.userId == user.Id && r.recipe.food.Name.ToLower() == FoodName.ToLower() && r.recipe.chef.UserName.ToLower() == ChefName.ToLower() && r.recipe.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
                if (IsExistScoreRecipeUser == null)
                {
                    _db.ScoreRecipeUsers.Add(new ScoreRecipeUser
                    {
                        recipe = Recipe,
                        RecipeId = Recipe.Id,
                        Rate = rate,
                        userId = user.Id,
                        user = user
                    });
                    InterGeneralResponse response=await _recipeService.RateRecipe(Recipe, rate);
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Add rate Successfully"
                    };
                }
                else
                {
                    IsExistScoreRecipeUser.Rate = rate;
                    _db.ScoreRecipeUsers.Update(IsExistScoreRecipeUser);
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Update rate Successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<InterGeneralResponse> DeleteScoreRecipeForUser(Chef user, string FoodName, string ChefName, string RecipeName, int rate)
        {
            try
            {
                var Recipe = await _recipeService.Search_Recipe(FoodName, ChefName, RecipeName);
                var IsExistScoreRecipeUser = _db.ScoreRecipeUsers.Where(r => r.userId == user.Id && r.recipe.food.Name.ToLower() == FoodName.ToLower() && r.recipe.chef.UserName.ToLower() == ChefName.ToLower() && r.recipe.Name.ToLower() == RecipeName.ToLower()).FirstOrDefault();
                if (IsExistScoreRecipeUser == null)
                {
                    return new InterGeneralResponse
                    {
                        IsSucceed = false,
                        Message = "This user does not rated for this recipe"
                    };
                }
                else
                {
                    _db.ScoreRecipeUsers.Remove(IsExistScoreRecipeUser);
                    InterGeneralResponse response = await _recipeService.DeleteRateRecipe(Recipe, rate);
                    _db.SaveChanges();
                    return new InterGeneralResponse
                    {
                        IsSucceed = true,
                        Message = "Delete rate Successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                return new InterGeneralResponse
                {
                    IsSucceed = false,
                    Message = ex.Message
                };
            }
        }
            //public async Task<IEnumerable<Food>> FavouritFoodByUserSortedRate(string userId)
            //{
            //    var Foods = await _db.FavouritFoodUsers.Where(r => r.userId == userId).OrderByDescending(r => r.Food.).Select(x => x.Food).ToListAsync();
            //    return Foods;
            //}

        }
}
