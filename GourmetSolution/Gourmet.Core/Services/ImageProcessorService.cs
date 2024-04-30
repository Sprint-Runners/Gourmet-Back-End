using Gourmet.Core.Domain.OtherObject;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
namespace Gourmet.Core.Services
{
    public class ImageProcessorService : IImageProcessorService
    {
        private readonly IWebHostEnvironment _environment;
        public ImageProcessorService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        private string GetFilePathUser(string Username)
        {
            return this._environment.WebRootPath + "\\Uploads\\User\\" + Username;
        }
        private string GetFilePathRecipe(string FoodName, string username,string Name)
        {
            return this._environment.WebRootPath + "\\Uploads\\Food\\" + FoodName + "\\"+username + "\\" + Name;
        }
        private string GetFilePathFood(string Name)
        {
            return this._environment.WebRootPath + "\\Uploads\\Food\\" + Name;
        }
        private string GetFilePathCategory(string CategoryName, string Name)
        {
            return this._environment.WebRootPath + "\\Uploads\\Category\\" + CategoryName + "\\" + Name;
        }
        public async Task<ImageResponse> UploadUserImage(IFormFile file, string username)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new ImageResponse
                    {
                        IsSucceed = true,
                        Message = "No file uploaded.",
                        ImagePath = null

                    };
                string Filename = username;
                string Filepath = GetFilePathUser(Filename);

                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\image.png";
                //age aks vojood dash
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await file.CopyToAsync(stream);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "uploade successful",
                    ImagePath = imagepath

                };
            }
            catch (Exception ex)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ex.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> UploadRecipeImage(IFormFile file, string FoodName, string username,string Name, int number)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new ImageResponse
                    {
                        IsSucceed = false,
                        Message = "No file uploaded.",
                        ImagePath = null

                    };
                //string Filename = Name;
                string Filepath = GetFilePathRecipe(FoodName, username,Name);

                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\image"+ number + ".png";
                //age aks vojood dash
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await file.CopyToAsync(stream);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "uploade successful",
                    ImagePath = imagepath

                };
            }
            catch (Exception ex)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ex.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> UploadFoodImage(IFormFile file, string Name)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new ImageResponse
                    {
                        IsSucceed = false,
                        Message = "No file uploaded.",
                        ImagePath = null

                    };
                string Filename = Name;
                string Filepath = GetFilePathFood(Filename);

                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\image.png";
                //age aks vojood dash
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await file.CopyToAsync(stream);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "uploade successful",
                    ImagePath = imagepath

                };
            }
            catch (Exception ex)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ex.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> UploadCategoryImage(IFormFile file, string CategoryName, string Name)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new ImageResponse
                    {
                        IsSucceed = true,
                        Message = "No file uploaded.",
                        ImagePath = null

                    };
                string Filename = Name;
                string Filepath = GetFilePathCategory(CategoryName, Filename);

                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\image.png";
                //age aks vojood dash
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await file.CopyToAsync(stream);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "uploade successful",
                    ImagePath = imagepath

                };
            }
            catch (Exception ex)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ex.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> RemoveUserImage(string Username)
        {
            string Filepath = GetFilePathUser(Username);
            string Imagepath = Filepath + "\\image.png";
            try
            {
                if (System.IO.File.Exists(Imagepath))
                {
                    System.IO.File.Delete(Imagepath);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "Delete successful",
                    ImagePath = null

                };
            }
            catch (Exception ext)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ext.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> RemoveRecipeImage(string FoodName, string username,string Name,int number)
        {
            string Filepath = GetFilePathRecipe(FoodName, username,Name);
            string Imagepath = Filepath + "\\image"+number+".png";
            try
            {
                if (System.IO.File.Exists(Imagepath))
                {
                    System.IO.File.Delete(Imagepath);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "Delete successful",
                    ImagePath = null

                };
            }
            catch (Exception ext)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ext.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> RemoveFoodImage(string Name)
        {
            string Filepath = GetFilePathFood(Name);
            string Imagepath = Filepath + "\\image.png";
            try
            {
                if (System.IO.File.Exists(Imagepath))
                {
                    System.IO.File.Delete(Imagepath);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "Delete successful",
                    ImagePath = null

                };
            }
            catch (Exception ext)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ext.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<ImageResponse> RemoveCategoryImage(string CategoryName, string Name)
        {
            string Filepath = GetFilePathCategory(CategoryName, Name);
            string Imagepath = Filepath + "\\image.png";
            try
            {
                if (System.IO.File.Exists(Imagepath))
                {
                    System.IO.File.Delete(Imagepath);
                }
                return new ImageResponse
                {
                    IsSucceed = true,
                    Message = "Delete successful",
                    ImagePath = null

                };
            }
            catch (Exception ext)
            {
                return new ImageResponse
                {
                    IsSucceed = false,
                    Message = ext.Message,
                    ImagePath = null

                };
            }
        }
        public async Task<string> GetImagebyUser(string username)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "http://185.129.119.228:4100";
            string Filepath = GetFilePathUser(username);
            string Imagepath = Filepath + "\\image.png";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/uploads/User/common/noimage.png";
            }
            else
            {
                ImageUrl = HostUrl + "/uploads/User/" + username + "/image.png";
            }
            return ImageUrl;

        }
        public async Task<string> GetImagebyRecipe(string FoodName, string username, string Name, int number)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "http://185.129.119.228:4100";
            string Filepath = GetFilePathRecipe(FoodName, username,Name);
            string Imagepath = Filepath + "\\image"+number+".png";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/uploads/common/noimage.png";
            }
            else
            {
                ImageUrl = HostUrl + "/uploads/Food/"  + FoodName + "/" + username + "/" + Name + "/image"+number+".png";
            }
            return ImageUrl;

        }
        public async Task<string> GetImagebyFood(string Name)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "http://185.129.119.228:4100";
            string Filepath = GetFilePathFood(Name);
            string Imagepath = Filepath + "\\image.png";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/uploads/common/noimage.png";
            }
            else
            {
                ImageUrl = HostUrl + "/uploads/Food/" + Name + "/image.png";
            }
            return ImageUrl;

        }
        public async Task<string> GetImagebyCategory(string CategoryName, string Name)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "http://185.129.119.228:4100";
            string Filepath = GetFilePathCategory(CategoryName, Name);
            string Imagepath = Filepath + "\\image.png";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/uploads/common/noimage.png";
            }
            else
            {
                ImageUrl = HostUrl + "/uploads/Category/" + CategoryName + "/" + Name + "/image.png";
            }
            return ImageUrl;

        }
    }
}

