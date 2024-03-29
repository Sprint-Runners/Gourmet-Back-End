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
    public class ChefService:IChefService
    {
        //private readonly AppDbContext _db;
        //public ChefService(AppDbContext db)
        //{
        //    _db = db;
        //}
        //public async Task<IEnumerable<Recipe>> GetRecipesByChefId(string chefId)
        //{
        //    var Recipes = await _db.Recipes.Where(r => r.ChefId == chefId).ToListAsync();
        //    return Recipes;
        //}

        //public async Task<double> GetChefScore(string chefId)
        //{
        //    var foods = await GetRecipesByChefId(chefId);
        //    double score= foods.Any() ? foods.Average(f => f.Score) : 0;
        //    //var chef= await _db.Chefs.FindAsync(chefId);
        //    //chef.Score = score;
        //    return score;
        //}
    }
}
