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
            var allRecipes = _db.Recipes.Where(r => r.food.Name.ToLower() == request.ToLower() && r.IsAccepted==true).OrderByDescending(r => r.Score).ToList();
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

        public async Task<List<Special_Foods>> Get_Special()
        {
            List<Special_Foods> specials = new List<Special_Foods>();
            //Quick Cook
            var isExistFood_Quick = _db.Foods.Where(r => r.Timetocook!=null && r.Timetocook<30);
            Special_Foods Special_Quick;
            if (isExistFood_Quick == null)
                Special_Quick = new Special_Foods { Success = false };
            else
            {
                var Category=_db.Special_Categories.Where(c => c.Title == "Quick").FirstOrDefault();
                Special_Quick = new Special_Foods
                {
                    Success = true,
                    Title = Category.Title,
                    Description = Category.Description,
                    Foods = isExistFood_Quick.ToList()
                };
            }
            specials.Add(Special_Quick);


            var isExistFood_Occasion = _db.Foods.Where(r => r.Special_Occasion != null && r.Special_Occasion == true);
            Special_Foods Special_Occasion;
            if (isExistFood_Occasion == null)
                Special_Occasion = new Special_Foods { Success = false };
            else
            {
                var Category = _db.Special_Categories.Where(c => c.Title == "Special").FirstOrDefault();
                Special_Occasion = new Special_Foods
                {
                    Success = true,
                    Title = Category.Title,
                    Description = Category.Description,
                    Foods = isExistFood_Occasion.ToList()
                };
            }
            specials.Add(Special_Occasion);

            var isExistFood_Breakfast = _db.Foods.Where(r => r.Is_breakfast != null && r.Is_breakfast == true);
            Special_Foods Special_Breakfast;
            if (isExistFood_Breakfast == null)
                Special_Breakfast = new Special_Foods { Success = false };
            else
            {
                var Category = _db.Special_Categories.Where(c => c.Title == "Breakfast").FirstOrDefault();
                Special_Breakfast = new Special_Foods
                {
                    Success = true,
                    Title = Category.Title,
                    Description = Category.Description,
                    Foods = isExistFood_Breakfast.ToList()
                };
            }
            specials.Add(Special_Breakfast);

            var isExistFood_Main = _db.Foods.Where(r => r.Is_Main != null && r.Is_Main == true);
            Special_Foods Special_Main;
            if (isExistFood_Main == null)
                Special_Main = new Special_Foods { Success = false };
            else
            {
                var Category = _db.Special_Categories.Where(c => c.Title == "Main").FirstOrDefault();
                Special_Main = new Special_Foods
                {
                    Success = true,
                    Title = Category.Title,
                    Description = Category.Description,
                    Foods = isExistFood_Main.ToList()
                };
            }
            specials.Add(Special_Main);
            return specials;
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
