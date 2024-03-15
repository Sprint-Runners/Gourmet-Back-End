using Gourmet.Core.Domain.Entities;
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

        public async Task<User> Sign_Up_User(SignUpRequest request)
        {
            User new_user=request.ToUser();
            _db.Users.Add(new_user);
            await _db.SaveChangesAsync();
            return new_user;
        }
    }
}
