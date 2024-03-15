using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.Domain.Entities;

namespace Gourmet.Core.ServiceContracts
{
    public interface IUsersService
    {
        Task<User> Sign_Up_User(SignUpRequest request);
    }
}
