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
        Task<ImageResponse> UploadRecipeImage(IFormFile file, string Name, string username);
        Task<ImageResponse> UploadFoodImage(IFormFile file, string Name);
        ImageResponse RemoveUserImage(string Username);
        ImageResponse RemoveRecipeImage(string Name, string username);
        ImageResponse RemoveFoodImage(string Name);
        string GetImagebyUser(string username);
        string GetImagebyRecipe(string Name, string username);
        string GetImagebyFood(string Name);
    }
}
