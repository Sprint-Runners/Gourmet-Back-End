using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gourmet.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IImageProcessorService _imageProcessorService;
        private readonly IIngredientService _ingredientService;
        private readonly IFoodService _foodService;
        private readonly ICategoriesService _categoriesService;
        private readonly IRecipeService _recipeService;
        private readonly IUsersService _usersService;
        public AdminController(IImageProcessorService imageProcessorService,IIngredientService ingredientService, IFoodService foodService, ICategoriesService categoriesService,IRecipeService recipeService,IUsersService usersService)
        {
            _imageProcessorService = imageProcessorService;
            _ingredientService = ingredientService;
            _foodService = foodService;
            _categoriesService = categoriesService;
            _recipeService = recipeService;
            _usersService = usersService;
        }
        [HttpPost]
        [Route("Add_Ingredient")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_Ingredient(AddIngredientRequest request)
        {
            try
            {
                var result = await _ingredientService.Create(request);
                if (result.IsSucceed)
                {
                    return Ok(result.Message);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_Food")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_Food(AddFoodRequest request)
        {
            try
            {
                var result = await _foodService.Create(request);
                if (result.IsSucceed)
                {
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadFoodImage(file,result.food.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.food.ImgeUrl = await _imageProcessorService.GetImagebyFood(result.food.Name);
                        return Ok(result.food);
                    }
                    return Problem(detail: ResultImage.Message, statusCode: 400);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_PSOI_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_PSOI_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreatePSOICategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "PSOI",result.PSOI.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.PSOI.ImageUrl = await _imageProcessorService.GetImagebyCategory("PSOI", result.PSOI.Name);
                        return Ok(result.PSOI);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }

        [HttpPost]
        [Route("Add_CM_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_CM_Category(AddCategoryRequest request)
        {
            try
            {
                Console.WriteLine("vkfhfjhgcgjhdcgddhd**********");
                var result = await _categoriesService.CreateCMCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "CM", result.CM.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.CM.ImageUrl = await _imageProcessorService.GetImagebyCategory("CM", result.CM.Name);
                        return Ok(result.CM);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_FT_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_FT_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateFTCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "FT", result.FT.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.FT.ImageUrl = await _imageProcessorService.GetImagebyCategory("FT", result.FT.Name);
                        return Ok(result.FT);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_N_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_N_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateNCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "N", result.N.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.N.ImageUrl = await _imageProcessorService.GetImagebyCategory("N", result.N.Name);
                        return Ok(result.N);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_MT_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_MT_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateMTCategory(request);
                if (result.IsSucceed)
                {
                    if (Request.Form.Files.Count == 0)
                    {
                        return Ok("Not Image Uploaded");
                    }
                    var file = Request.Form.Files[0];
                    var ResultImage = await _imageProcessorService.UploadCategoryImage(file, "MT", result.MT.Name);
                    if (ResultImage.IsSucceed)
                    {
                        result.MT.ImageUrl = await _imageProcessorService.GetImagebyCategory("MT", result.MT.Name);
                        return Ok(result.MT);
                    }
                    //return Problem(detail: ResultImage.Message, statusCode: 400);
                    return Ok("Not Image Uploaded");
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Add_DL_Category")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Add_DL_Category(AddCategoryRequest request)
        {
            try
            {
                var result = await _categoriesService.CreateDLCategory(request);
                if (result.IsSucceed)
                {
                    return Ok(result.DL);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Accept_recipe")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Accept_recipe(AcceptedRecipeRequest request)
        {
            try
            {
                var result = await _recipeService.AcceptedRecipe(request.FoodName, request.username, request.Name);
                if (result.IsSucceed)
                {
                    return Ok(new GeneralResponse
                    {
                        Message = "Recipe Accepted"
                    });
                }
                return Problem(detail:result.Message,statusCode:400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPost]
        [Route("Reject_recipe")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)]
        public async Task<IActionResult> Reject_recipe(AcceptedRecipeRequest request)
        {
            try
            {
                var result = await _recipeService.RejectedRecipe(request.FoodName, request.username, request.Name);
                if (result.IsSucceed)
                {
                    return Ok(new GeneralResponse
                    {
                        Message = "Recipe Rejected"
                    });
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        [HttpPut]
        [Route("BanUser")]
        public async Task<IActionResult> Ban_User_ByUsername(BanUserRequest request)
        {
            
            try
            {
                var result = await _usersService.BanUser(request);
                if (result.IsSucceed)
                {
                    return Ok(result);
                }
                return Problem(detail: result.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 400);
            }
        }
        //ino vase recipe haee ke kamel naboode bayad benevisi va kamel koni
        //[HttpPost]
        //[Route("Add_Ingredient")]
        //[Authorize(Roles = StaticUserRoles.CHEF)]
        //public async Task<IActionResult> Add_Ingredient(AddIngredientRequest request)
        //{

        //}
    }
}
