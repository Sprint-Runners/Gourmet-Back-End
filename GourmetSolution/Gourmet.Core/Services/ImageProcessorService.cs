using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace Gourmet.Core.Services
{
    public class ImageProcessorService:IImageProcessorService
    {
    //    private readonly IWebHostEnvironment _environment;
    //    public ImageProcessorService(IWebHostEnvironment environment)
    //    {
    //        _environment = environment;
    //    }
    //    private string GetFilePathUser(string Username)
    //    {
    //        return this._environment.WebRootPath + "\\Uploads\\User\\" + Username;
    //    }
    //    private string GetFilePathFood(string Name,string username)
    //    {
    //        return this._environment.WebRootPath + "\\Uploads\\Food\\" + username + "\\" + Name;
    //    }
    //    public async Task<ImageResponse> UploadUserImage(IFormFile file,string username)
    //    {
    //        try
    //        {
    //            if (file == null || file.Length == 0)
    //                return new ImageResponse
    //                {
    //                    IsSucceed = false,
    //                    Message = "No file uploaded.",
    //                    ImagePath = null

    //                };
    //            string Filename = username;
    //            string Filepath = GetFilePathUser(Filename);

    //            if (!System.IO.Directory.Exists(Filepath))
    //            {
    //                System.IO.Directory.CreateDirectory(Filepath);
    //            }

    //            string imagepath = Filepath + "\\image.png";
    //            //age aks vojood dash
    //            if (System.IO.File.Exists(imagepath))
    //            {
    //                System.IO.File.Delete(imagepath);
    //            }
    //            using (FileStream stream = System.IO.File.Create(imagepath))
    //            {
    //                await file.CopyToAsync(stream);
    //            }
    //            return new ImageResponse
    //            {
    //                IsSucceed = true,
    //                Message = "uploade successful",
    //                ImagePath = imagepath

    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ImageResponse
    //            {
    //                IsSucceed = false,
    //                Message = ex.Message,
    //                ImagePath = null

    //            };
    //        }
    //    }
    //    public async Task<ImageResponse> UploadFoodImage(IFormFile file, string Name,string username)
    //    {
    //        try
    //        {
    //            if (file == null || file.Length == 0)
    //                return new ImageResponse
    //                {
    //                    IsSucceed = false,
    //                    Message = "No file uploaded.",
    //                    ImagePath = null

    //                };
    //            string Filename = Name;
    //            string Filepath = GetFilePathFood(Filename,username);

    //            if (!System.IO.Directory.Exists(Filepath))
    //            {
    //                System.IO.Directory.CreateDirectory(Filepath);
    //            }

    //            string imagepath = Filepath + "\\image.png";
    //            //age aks vojood dash
    //            if (System.IO.File.Exists(imagepath))
    //            {
    //                System.IO.File.Delete(imagepath);
    //            }
    //            using (FileStream stream = System.IO.File.Create(imagepath))
    //            {
    //                await file.CopyToAsync(stream);
    //            }
    //            return new ImageResponse
    //            {
    //                IsSucceed = true,
    //                Message = "uploade successful",
    //                ImagePath = imagepath

    //            };
    //        }
    //        catch (Exception ex)
    //        {
    //            return new ImageResponse
    //            {
    //                IsSucceed = false,
    //                Message = ex.Message,
    //                ImagePath = null

    //            };
    //        }
    //    }
    //    public ImageResponse RemoveUserImage(string Username)
    //    {
    //        string Filepath = GetFilePathUser(Username);
    //        string Imagepath = Filepath + "\\image.png";
    //        try
    //        {
    //            if (System.IO.File.Exists(Imagepath))
    //            {
    //                System.IO.File.Delete(Imagepath);
    //            }
    //            return new ImageResponse
    //            {
    //                IsSucceed = true,
    //                Message = "Delete successful",
    //                ImagePath = null

    //            };
    //        }
    //        catch (Exception ext)
    //        {
    //            return new ImageResponse
    //            {
    //                IsSucceed = false,
    //                Message = ext.Message,
    //                ImagePath = null

    //            };
    //        }
    //    }
    //    public ImageResponse RemoveFoodImage(string Name, string username)
    //    {
    //        string Filepath = GetFilePathFood(Name,username);
    //        string Imagepath = Filepath + "\\image.png";
    //        try
    //        {
    //            if (System.IO.File.Exists(Imagepath))
    //            {
    //                System.IO.File.Delete(Imagepath);
    //            }
    //            return new ImageResponse
    //            {
    //                IsSucceed = true,
    //                Message = "Delete successful",
    //                ImagePath = null

    //            };
    //        }
    //        catch (Exception ext)
    //        {
    //            return new ImageResponse
    //            {
    //                IsSucceed = false,
    //                Message = ext.Message,
    //                ImagePath = null

    //            };
    //        }
    //    }
    //    public string GetImagebyUser(string username)
    //    {
    //        string ImageUrl = string.Empty;
    //        string HostUrl = "http://localhost:5286";
    //        string Filepath = GetFilePathUser(username);
    //        string Imagepath = Filepath + "\\image.png";
    //        if (!System.IO.File.Exists(Imagepath))
    //        {
    //            ImageUrl = HostUrl + "/uploads/common/noimage.png";
    //        }
    //        else
    //        {
    //            ImageUrl = HostUrl + "/uploads/User/" + username + "/image.png";
    //        }
    //        return ImageUrl;

    //    }
    //    public string GetImagebyFood(string Name,string username)
    //    {
    //        string ImageUrl = string.Empty;
    //        string HostUrl = "http://localhost:5286";
    //        string Filepath = GetFilePathFood(Name, username);
    //        string Imagepath = Filepath + "\\image.png";
    //        if (!System.IO.File.Exists(Imagepath))
    //        {
    //            ImageUrl = HostUrl + "/uploads/common/noimage.png";
    //        }
    //        else
    //        {
    //            ImageUrl = HostUrl + "/uploads/Food/" +username +"/"+Name + "/image.png";
    //        }
    //        return ImageUrl;

    //    }
    }
}

