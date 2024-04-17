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
        Task<Response> Sign_Up_User(SignUpRequest request);
        Task<Response> LoginAsync(LoginRequest request);
        Task<Response> MakeAdminAsync(UpdatePermissionRequest updatePermission);
        Task<Email_Response> Authenticate_Email(SignUpRequest request);
        Task<Response> SeedRolesAsync();
        Task<Email_Response> Temproary_Password(Add_Temp_Password request);
    }
}