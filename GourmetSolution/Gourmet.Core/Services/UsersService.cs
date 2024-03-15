using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Gourmet.Infrastructure.GourmetDbcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _db;
        public UsersService(ApplicationDbContext db)
        {
            _db=db;
        }

        public Task<AuthenticationResponse> Sign_Up_User(SignUpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
