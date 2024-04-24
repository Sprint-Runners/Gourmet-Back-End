using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace Gourmet.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Chef> _userManager;
        private readonly AppDbContext _db;
        public UserService(UserManager<Chef> userManager,AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<Response> Edit(EditUserRequest request,string username)
        {
            var isExistsUser = await _userManager.FindByNameAsync(username);
            if (isExistsUser == null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "UserName not Exists",
                    user = null
                };
               
            Chef EditUser = (Chef)isExistsUser;
            EditUser.Email = request.Email;
            EditUser.FullName = request.FullName;
            EditUser.PhoneNumber = request.PhoneNumber;
            EditUser.Aboutme= request.Aboutme;
            //EditUser.UserName
            var result = await _userManager.UpdateAsync(EditUser);

            if (result.Succeeded)
                return new Response()
                {
                    IsSucceed = true,
                    Message = "Update Successfully",
                    user = EditUser
                };
            var errorString = "User Updat Failed Beacause: ";
            foreach (var error in result.Errors)
            {
                errorString += " # " + error.Description;
            }
            return new Response()
            {
                IsSucceed = false,
                Message = errorString,
                user = null
            };
        }
        public async Task<Response> Read(string username)
        {
            var isExistsUser = await _userManager.FindByNameAsync(username);

            if (isExistsUser == null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "UserName not Exists",
                    user = null
                };
            return new Response()
            {
                IsSucceed = true,
                Message = "Read Successfully",
                user = isExistsUser
            };

        }
        public async Task<IEnumerable<Recipe>> FavouritRecipeByUser(string userId)
        {
            var Foods = await _db.FavouritRecipeUsers.Where(r => r.userId == userId).OrderByDescending(r=>r.TimeToLike).Select(x=>x.recipe).ToListAsync();
            return Foods;
        }
        //public async Task<IEnumerable<Food>> FavouritFoodByUserSortedRate(string userId)
        //{
        //    var Foods = await _db.FavouritFoodUsers.Where(r => r.userId == userId).OrderByDescending(r => r.Food.).Select(x => x.Food).ToListAsync();
        //    return Foods;
        //}
        public async Task<IEnumerable<Recipe>> RecentRecipeByUser(string userId)
        {
            var Foods = await _db.RecentRecipeUsers.Where(r => r.userId == userId).OrderByDescending(r => r.VisitTime).Select(x => x.recipe).ToListAsync();
            return Foods;
        }
    }
}
