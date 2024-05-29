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
using System.Net.Mail;
using System.Net;
using static System.Net.Mime.MediaTypeNames;


namespace Gourmet.Core.Services
{

    public class UsersService : IUsersService
    {
        private static string Email_Address;
        private static string Email_Password;
        //private readonly UserManager<IdentityUser> _userManager;
        private readonly UserManager<Chef> _userManager;
        //private readonly UserManager<Chef> _userManager2;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        public UsersService(AppDbContext db, UserManager<Chef> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            Email_Address = "GourmetFoodWebSite@gmail.com";
            Email_Password = _db.Secrets.Find(Email_Address).Password;
        }
        public async Task<UserResponse> Sign_Up_User(SignUpRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.Email);

            if (isExistsUser != null)
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = "UserName Already Exists",
                    user = null
                };
            Chef new_user = new Chef()
            {

                UserName = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var user = _db.Email_Passwords.Find(request.Email);

            if (user == null || user.Temp_Password.ToString() != request.Temp_Code)
                return new UserResponse()
                {
                    IsSucceed = false,
                    user = null,
                    Message = "The Authentication code or the email is incorrect"
                };

            var createUserResult = await _userManager.CreateAsync(new_user, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Beacause: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = errorString,
                    user = null
                };
            }
            await _userManager.AddToRoleAsync(new_user, StaticUserRoles.USER);

            return new UserResponse()
            {
                IsSucceed = true,
                Message = "User Created Successfully",
                user = new_user
            };
        }

        public async Task<UserResponse> LoginAsync(LoginRequest request)
        {
            var new_user = await _userManager.FindByNameAsync(request.username);

            if (new_user is null)
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = "Invalid Credentials",
                    user = null
                };

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(new_user, request.password);

            if (!isPasswordCorrect)
            {
                var Temp = await _db.Temproary_Passwords.FindAsync(request.username);
                if (Temp is null || Temp.Password != request.password)
                    return new UserResponse()
                    {
                        IsSucceed = false,
                        Message = "Invalid Credentials",
                        user = null
                    };
                else
                {
                    _db.Temproary_Passwords.Remove(Temp);
                    _db.SaveChanges();
                }

            }

            return new UserResponse()
            {
                IsSucceed = true,
                Message = "Login was successful",
                user = (Chef)new_user
            };
        }

        public async Task<UserResponse> MakeAdminAsync(UpdatePermissionRequest updatePermission)
        {
            var new_user = await _userManager.FindByNameAsync(updatePermission.UserName);

            if (new_user is null)
                return new UserResponse()
                {
                    IsSucceed = false,
                    Message = "Invalid User name!",
                    user = null
                };

            await _userManager.AddToRoleAsync(new_user, StaticUserRoles.ADMIN);

            return new UserResponse()
            {
                IsSucceed = true,
                user = (Chef)new_user,
                Message = "User is now an ADMIN"
            };
        }

        public async Task<UserResponse> SeedRolesAsync()
        {
            bool isChefRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.CHEF);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (isChefRoleExists && isAdminRoleExists && isUserRoleExists)
                return new UserResponse()
                {
                    IsSucceed = true,
                    Message = "Roles Seeding is Already Done"
                };

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.CHEF));

            return new UserResponse()
            {
                IsSucceed = true,
                Message = "Role Seeding Done Successfully"
            };
        }

        public async Task<Email_Response> Authenticate_Email(Authrequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.Email);

            if (isExistsUser != null)
                return new Email_Response() { IsSucceed = false, Message = "The username already exists" };

            MailAddress From = new MailAddress(Email_Address);
            MailAddress To = new MailAddress(request.Email);
            Random random = new Random();
            int Temp_Pass = random.Next(10000, 99999);

            string Body_Message = "Your Security Code is " + Temp_Pass.ToString();
            MailMessage message = new MailMessage(From, To) { Subject = "Gourmet Authentication", Body = Body_Message };
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(Email_Address, Email_Password);
            Email_Pass Response = new Email_Pass() { Email = request.Email, Temp_Password = Temp_Pass };
            var sample = _db.Email_Passwords.Find(request.Email);
            if (sample != null)
                _db.Email_Passwords.Remove(sample);
            try
            {

                var res = await _db.Email_Passwords.AddAsync(Response);
                _db.SaveChanges();
                smtpClient.Send(message);
                Email_Response response = new Email_Response() { IsSucceed = true, Message = "Email has been sent" };
                return response;
            }
            catch (Exception ex)
            {
                _db.Email_Passwords.Remove(Response);
                Email_Response response = new Email_Response() { IsSucceed = false, Message = "Email could not be sent"+ex.Message };
                _db.SaveChanges();
                return response;
            }
        }

        public async Task<Email_Response> Temproary_Password(Add_Temp_Password request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.Email);

            if (isExistsUser == null)
                return new Email_Response() { IsSucceed = false, Message = "The Email does not exist" };
            var temp = await _db.Temproary_Passwords.FindAsync(request.Email);
            if (temp != null)
                _db.Remove(temp);
            MailAddress From = new MailAddress(Email_Address);
            MailAddress To = new MailAddress(request.Email);
            Random random = new Random();
            int Temp_Pass = random.Next(10000, 99999);
            string password = "Gourmet$" + Temp_Pass.ToString();
            string Body_Message = "Your Temproary password is " + password + "\nPlease change your password once you log in";
            MailMessage message = new MailMessage(From, To) { Subject = "Gourmet Temproary Password", Body = Body_Message };
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(Email_Address, Email_Password);
            try
            {
                Temp_Password Response = new Temp_Password() { Email = request.Email, Password = password };
                
                var res = await _db.Temproary_Passwords.AddAsync(Response);
                _db.SaveChanges();
               
                smtpClient.Send(message);
                Email_Response response = new Email_Response() { IsSucceed = true, Message = "Email has been sent" };
                return response;
            }
            catch (Exception ex)
            {
                Email_Response response = new Email_Response() { IsSucceed = false, Message = "Email could not be sent" };
                return response;
            }
        }

        public async Task<BanUserResponse> BanUser(BanUserRequest request)
        {
            var isExistsUser = await _userManager.FindByNameAsync(request.UserName);
            if (isExistsUser == null)
                return new BanUserResponse() { IsSucceed = false, Message = "No user with this username exists" };
            isExistsUser.Ban = true;
            //EditUser.UserName
            var result = await _userManager.UpdateAsync(isExistsUser);

            if (result.Succeeded)
                return new BanUserResponse()
                {
                    IsSucceed = true,
                    Message = "User Banned successfuly"
                };
            else
                return new BanUserResponse()
                {
                    IsSucceed = false,
                    Message = "Could not ban user successfuly"
                };
        }

    }
}