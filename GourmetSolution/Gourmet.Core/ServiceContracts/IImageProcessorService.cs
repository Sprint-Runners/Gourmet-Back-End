using Gourmet.Core.Domain.OtherObject;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.ServiceContracts
{
    public interface IImageProcessorService
    {
        Task<ImageResponse> UploadUserImage(IFormFile file, string username);
        Task<ImageResponse> UploadRecipeImage(IFormFile file, string FoodName, string username,string Name);
        Task<ImageResponse> UploadFoodImage(IFormFile file, string Name);
        Task<ImageResponse> UploadCategoryImage(IFormFile file, string CategoryName,string Name);
         Task<ImageResponse> RemoveUserImage(string Username);
         Task<ImageResponse> RemoveRecipeImage(string FoodName, string username,string Name);
        Task<ImageResponse> RemoveFoodImage(string Name);
        Task<ImageResponse> RemoveCategoryImage(string CategoryName,string Name);
          Task<string> GetImagebyUser(string username);
         Task<string> GetImagebyRecipe(string FoodName, string username,string Name);
          Task<string> GetImagebyFood(string Name);
         Task<string> GetImagebyCategory(string CategotyName,string Name);
    }
}
