using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;


namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ChefService _chefservice;
        public HomeController(AppDbContext db, ChefService chefService)
        {
            _db = db;
            _chefservice = chefService;
        }
        [HttpGet("DailyOffer")]
        public async Task<IEnumerable<Food>> GetِDailyOfferFromDatabaseAsync()
        {
            var random = new Random();
            var allIds = await _db.Foods.Select(x => x.Id).ToListAsync();
            var randomIds = allIds.OrderBy(x => random.Next()).Take(3).ToList();

            var randomRows = await _db.Foods.Where(x => randomIds.Contains(x.Id)).ToListAsync();
            return randomRows;
        }
        [HttpGet("TopChef")]
        public async Task<IEnumerable<Chef>> GetTopChefFromDatabaseAsync()
        {
            var chefs = await _db.Chefs.ToListAsync();

            var topChefs = chefs.OrderByDescending(async c => await _chefservice.GetChefScore(c.Id))
                                .Take(3)
                                .ToList();

            return topChefs;
        }
        //public async Task<IEnumerable<>> GetAllCategories()
        //{

        //}
    }
}


