using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Gourmet.Core.DTO.Response;
using System.Xml.Linq;
using Gourmet.Core.ServiceContracts;


namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IChefService _chefservice;
        private readonly IImageProcessorService _imageProcessorService;
        public HomeController(AppDbContext db, IChefService chefService, IImageProcessorService imageProcessorService)
        {
            _db = db;
            _chefservice = chefService;
            _imageProcessorService = imageProcessorService;
        }
        [HttpGet("DailyOffer")]
        public async Task<IActionResult> GetِDailyOfferFromDatabaseAsync()
        {
            try
            {
                var random = new Random();
                var allIds = await _db.Foods.Select(x => x.Id).ToListAsync();
                var randomIds = allIds.OrderBy(x => random.Next()).Take(3).ToList();

                var randomRows = await _db.Foods.Where(x => randomIds.Contains(x.Id)).ToListAsync();
                List<FoodInformationResponse> randomFood = new List<FoodInformationResponse>();
                foreach (Food row in randomRows)
                {
                    row.ImgeUrl = _imageProcessorService.GetImagebyFood(row.Name);
                    randomFood.Add(new FoodInformationResponse
                    {
                        Name = row.Name,
                        ImagePath = row.ImgeUrl
                    }); ;
                }
                return Ok(randomFood);
            }
            catch(Exception ex)
            {
                return Problem(detail:ex.Message, statusCode: 400);
            }
        }
        [HttpGet("TopChef")]
        public async Task<IActionResult> GetTopChefFromDatabaseAsync()
        {
            try
            {
                var chefs = await _db.Chefs.ToListAsync();

                var topChefs = chefs.OrderByDescending(async c => await _chefservice.GetChefScore(c.Id))
                                    .Take(3)
                                    .ToList();
                List<TopChefResponse> TopChefs = new List<TopChefResponse>();
                foreach (Chef row in topChefs)
                {
                    row.ImageURL = _imageProcessorService.GetImagebyUser(row.UserName);
                    TopChefs.Add(new TopChefResponse
                    {
                        Name = row.UserName,
                        Score = row.Score,
                        ImagePath = row.ImageURL
                    }); ;
                }
                return Ok(TopChefs);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        //public async Task<IEnumerable<>> GetAllCategories()
        //{

        //}
    }
}


