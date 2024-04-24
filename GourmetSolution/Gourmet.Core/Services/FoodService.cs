using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Services
{
    public class FoodService : IFoodService
    {
        private readonly AppDbContext _db;
        public FoodService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<FoodResponse> Create(AddFoodRequest request)
        {
            var isExistFood = _db.Foods.Where(r => r.Name.ToLower() == request.Name.ToLower()).FirstOrDefault();
            if (isExistFood != null)
            {
                return new FoodResponse
                {
                    IsSucceed = false,
                    Message = "This Food Already Exists",
                    food=null
                };
            }
            Food food = new Food
            {
                Id = new Guid(),
                Name = request.Name.ToLower(),

            };
            _db.Foods.Add(food);
            _db.SaveChanges();
            return new FoodResponse
            {
                IsSucceed = true,
                Message = "Food Added Successfully",
                food=food
            };
        }
        public async Task<SearchRecipesFood> GetAllRecipe(string request)
        {
            var isExistFood = _db.Foods.Where(r => r.Name.ToLower() == request.ToLower()).FirstOrDefault();
            if (isExistFood == null)
            {
                return new SearchRecipesFood
                {
                    IsSucceed = false,
                    Message = "This Food Not Exists",
                    Recipes = null
                };
            }
            var allRecipes = _db.Recipes.Where(r => r.food.Name.ToLower() == request.ToLower()).ToList();
            if (allRecipes.Count == 0)
            {
                return new SearchRecipesFood
                {
                    IsSucceed = false,
                    Message = "No recipes have been added for this food yet",
                    Recipes = null
                };
            }
            return new SearchRecipesFood
            {
                IsSucceed = true,
                Message = "Recipes found successfully",
                Recipes = allRecipes
            };
        }
            //public async Task<IEnumerable<Food>> GetAllFoodWithOnePSOI(Guid category)
            //{
            //    var foods = await _db.Foods.Where(f => f.Primary_Source_of_IngredientId == category).ToListAsync();
            //    return foods;
            //}
            //public async Task<IEnumerable<Food>> GetAllFoodWithOneCM(Guid category)
            //{
            //    var foods = await _db.Foods.Where(f => f.Cooking_MethodId == category).ToListAsync();
            //    return foods;
            //}
            //public async Task<IEnumerable<Food>> GetAllFoodWithOneFT(Guid category)
            //{
            //    var foods = await _db.Foods.Where(f => f.Food_typeId == category).ToListAsync();
            //    return foods;
            //}
            //public async Task<IEnumerable<Food>> GetAllFoodWithOneN(Guid category)
            //{
            //    var foods = await _db.Foods.Where(f => f.NationalityId == category).ToListAsync();
            //    return foods;
            //}
            //public async Task<IEnumerable<Food>> GetAllFoodWithONeMT(Guid category)
            //{
            //    var foods = await _db.Foods.Where(f => f.Meal_TypeId == category).ToListAsync();
            //    return foods;
            //}

        }
}
