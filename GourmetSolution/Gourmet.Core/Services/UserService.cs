using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace Gourmet.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response> Edit(EditUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.oldusername);

            if (isExistsUser != null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "UserName not Exists",
                    user = null
                };
            var isExitNewUsername = await _userManager.FindByEmailAsync(request.newusername);
            if (isExitNewUsername != null && request.oldusername != isExitNewUsername.UserName)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "NewUserName is already exist",
                    user = null
                };
            ApplicationUser EditUser = (ApplicationUser)isExistsUser;
            EditUser.UserName = request.newusername;
            EditUser.Email = request.Email;
            EditUser.FirstName = request.FirstName;
            EditUser.LastName = request.LastName;
            EditUser.PhoneNumber = request.PhoneNumber;

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
        public async Task<Response> Read(ReadUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);

            if (isExistsUser != null)
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
    }
}
