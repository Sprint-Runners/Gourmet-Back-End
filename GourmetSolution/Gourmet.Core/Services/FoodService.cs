using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
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
        public async Task<IEnumerable<Food>> GetAllFoodWithOnePSOI(Guid category)
        {
            var foods = await _db.Foods.Where(f => f.Primary_Source_of_IngredientId == category).ToListAsync();
            return foods;
        }
        public async Task<IEnumerable<Food>> GetAllFoodWithOneCM(Guid category)
        {
            var foods = await _db.Foods.Where(f => f.Cooking_MethodId == category).ToListAsync();
            return foods;
        }
        public async Task<IEnumerable<Food>> GetAllFoodWithOneFT(Guid category)
        {
            var foods = await _db.Foods.Where(f => f.Food_typeId == category).ToListAsync();
            return foods;
        }
        public async Task<IEnumerable<Food>> GetAllFoodWithOneN(Guid category)
        {
            var foods = await _db.Foods.Where(f => f.NationalityId == category).ToListAsync();
            return foods;
        }
        public async Task<IEnumerable<Food>> GetAllFoodWithONeMT(Guid category)
        {
            var foods = await _db.Foods.Where(f => f.Meal_TypeId == category).ToListAsync();
            return foods;
        }

    }
}
