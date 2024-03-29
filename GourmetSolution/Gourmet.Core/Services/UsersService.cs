using Gourmet.Core.DataBase.GourmetDbcontext;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Gourmet.Core.Services
{

    public class UsersService : IUsersService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        public UsersService(AppDbContext db,UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<Response> Sign_Up_User(SignUpRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.Email);

            if (isExistsUser != null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists",
                    user = null
                };
            IdentityUser new_user = new IdentityUser()
            {

                UserName = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var createUserResult = await _userManager.CreateAsync(new_user, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
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
            await _userManager.AddToRoleAsync(new_user, StaticUserRoles.USER);

            return new Response()
            {
                IsSucceed = true,
                Message = "User Created Successfully",
                user = new_user
            };
        }

        public async Task<Response> LoginAsync(LoginRequest request)
        {
            var new_user = await _userManager.FindByNameAsync(request.username);

            if (new_user is null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials",
                    user = null
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(new_user, request.password);

            if (!isPasswordCorrect)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials",
                    user = null
                };

            return new Response()
            {
                IsSucceed = true,
                Message = "Login was successful",
                user = (IdentityUser)new_user
            };
        }

        public async Task<Response> MakeAdminAsync(UpdatePermissionRequest updatePermission)
        {
            var new_user = await _userManager.FindByNameAsync(updatePermission.UserName);

            if (new_user is null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "Invalid User name!",
                    user = null
                };

            await _userManager.AddToRoleAsync(new_user, StaticUserRoles.ADMIN);

            return new Response()
            {
                IsSucceed = true,
                user = (IdentityUser)new_user,
                Message = "User is now an ADMIN"
            };
        }
        public async Task<Response> MakeChefAsync(UpdatePermissionRequest updatePermission)
        {
            var new_user = await _userManager.FindByNameAsync(updatePermission.UserName);

            if (new_user is null)
                return new Response()
                {
                    IsSucceed = false,
                    Message = "Invalid User name!",
                    user = null
                };

            await _userManager.AddToRoleAsync(new_user, StaticUserRoles.CHEF);
            //Chef chef = (Chef)new_user;
            return new Response()
            {
                IsSucceed = true,
                user = (IdentityUser)new_user,
                Message = "User is now an CHEF"
            };

        }
        public async Task<Response> SeedRolesAsync()
        {
            bool isChefRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.CHEF);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isChefRoleExists && isAdminRoleExists && isUserRoleExists)
                return new Response()
                {
                    IsSucceed = true,
                    Message = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.CHEF));

            return new Response()
            {
                IsSucceed = true,
                Message = "Role Seeding Done Successfully"
            };
        }
    }
}