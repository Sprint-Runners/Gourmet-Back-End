using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.Domain.Other_Object;

namespace Gourmet.Core.ServiceContracts
{
    public interface IUsersService
    {
        Task<UserResponse> Sign_Up_User(SignUpRequest request);
        Task<UserResponse> LoginAsync(LoginRequest request);
        Task<UserResponse> MakeAdminAsync(UpdatePermissionRequest updatePermission);
        Task<Email_Response> Authenticate_Email(Authrequest request);
        Task<UserResponse> SeedRolesAsync();
        Task<Email_Response> Temproary_Password(Add_Temp_Password request);
        Task<BanUserResponse> BanUser(BanUserRequest request);
        Task<Email_Response> Email_User(string username, string reason);
    }
}