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
    public class CategoriesService : ICategoriesService
    {
        private readonly AppDbContext _db;
        public CategoriesService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Primary_Source_of_Ingredient>> GetAllPSOICategory()
        {
            var PSOIs = await _db.PSOIs.ToListAsync();
            return PSOIs;
        }
        public async Task<IEnumerable<Cooking_Method>> GetAllCMCategory()
        {
            var CMs = await _db.CMs.ToListAsync();
            return CMs;
        }
        public async Task<IEnumerable<Food_type>> GetAllFTCategory()
        {
            var FTs = await _db.FTs.ToListAsync();
            return FTs;
        }
        public async Task<IEnumerable<Nationality>> GetAllNCategory()
        {
            var Ns = await _db.Ns.ToListAsync();
            return Ns;
        }
        public async Task<IEnumerable<Meal_Type>> GetAllMTCategory()
        {
            var MTs = await _db.MTs.ToListAsync();
            return MTs;
        }
    }
}
