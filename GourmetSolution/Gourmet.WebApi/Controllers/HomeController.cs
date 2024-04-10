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
        private readonly ICategoriesService _categoriesService;
        public HomeController(AppDbContext db, IChefService chefService, IImageProcessorService imageProcessorService,ICategoriesService categoriesService)
        {
            _db = db;
            _chefservice = chefService;
            _imageProcessorService = imageProcessorService;
            _categoriesService = categoriesService;
        }
        [HttpGet("Home")]
        public async Task<IActionResult> HomeAsync()
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
                    row.ImgeUrl = await _imageProcessorService.GetImagebyFood(row.Name);
                    randomFood.Add(new FoodInformationResponse
                    {
                        Name = row.Name,
                        ImagePath = row.ImgeUrl
                    }); ;
                }
                var chefs = await _db.Chefs.ToListAsync();
                var topChefs = chefs.OrderByDescending(async c => await _chefservice.GetChefScore(c.Id))
                                    .Take(3)
                                    .ToList();
                List<TopChefResponse> TopChefs = new List<TopChefResponse>();
                foreach (Chef row in topChefs)
                {
                    row.ImageURL = await _imageProcessorService.GetImagebyUser(row.UserName);
                    TopChefs.Add(new TopChefResponse
                    {
                        Name = row.UserName,
                        Score = row.Score,
                        ImagePath = row.ImageURL
                    }); ;
                }
                var PSOIS =await _categoriesService.GetAllPSOICategory();
                var FTS = await _categoriesService.GetAllFTCategory();
                var MTS = await _categoriesService.GetAllMTCategory();
                List<CategoriesResponse> Categories = new List<CategoriesResponse>();
                foreach(var category in PSOIS)
                {
                    Categories.Add(new CategoriesResponse
                    {
                        Name=category.Name,
                        CategoryName="Primary source of ingredient",
                        ImageUrl=await _imageProcessorService.GetImagebyCategory("PSOI",category.Name)

                    });
                }
                foreach (var category in FTS)
                {
                    Categories.Add(new CategoriesResponse
                    {
                        Name = category.Name,
                        CategoryName = "Food Type",
                        ImageUrl = await _imageProcessorService.GetImagebyCategory("FT", category.Name)

                    });
                }
                foreach (var category in MTS)
                {
                    Categories.Add(new CategoriesResponse
                    {
                        Name = category.Name,
                        CategoryName = "Meal Type",
                        ImageUrl = await _imageProcessorService.GetImagebyCategory("MT", category.Name)

                    });
                }
                var random_category = new Random();
                Categories = Categories.OrderBy(x => random_category.Next()).ToList();
                //return Ok(TopChefs);
                return Ok((randomFood, TopChefs,Categories));
            }
            catch(Exception ex)
            {
                return Problem(detail:ex.Message, statusCode: 400);
            }
        }
        //public async Task<IEnumerable<>> GetAllCategories()
        //{

        //}
        //[HttpGet("TopChef")]
        //public async Task<IActionResult> GetTopChefFromDatabaseAsync()
        //{
        //    try
        //    {
        //        var chefs = await _db.Chefs.ToListAsync();
        //        var topChefs = chefs.OrderByDescending(async c => await _chefservice.GetChefScore(c.Id))
        //                            .Take(3)
        //                            .ToList();
        //        List<TopChefResponse> TopChefs = new List<TopChefResponse>();
        //        foreach (Chef row in topChefs)
        //        {
        //            row.ImageURL = _imageProcessorService.GetImagebyUser(row.UserName);
        //            TopChefs.Add(new TopChefResponse
        //            {
        //                Name = row.UserName,
        //                Score = row.Score,
        //                ImagePath = row.ImageURL
        //            }); ;
        //        }
        //        return Ok(TopChefs);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem(detail: ex.Message, statusCode: 400);
        //    }
        //}

    }
}


