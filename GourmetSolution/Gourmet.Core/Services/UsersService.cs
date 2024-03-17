using Gourmet.Core.Domain.Entities;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.Helpers;
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

        public async Task<User> Login_User(Login_Request request)
        {
            HashHelper hashHelper = new HashHelper();
            string Hash_Pass = hashHelper.HashString(request.Password);

            List<User> list=_db.Users.Select(user => user).Where(user => user.Email == request.Email && user.Password == Hash_Pass)
                .ToList();
            if (list.Count == 0)
                throw new Exception("The user could not be found");
            return list[0];
        }

        public async Task<User> Sign_Up_User(SignUpRequest request)
        {
            User new_user=request.ToUser();
            if (_db.Users.Select(user => user).Where(user=>user.Email==new_user.Email).Count()>0)
                throw new ArgumentException("An account has already been registered with this email");
            HashHelper hashHelper = new HashHelper();
            new_user.Password = hashHelper.HashString(new_user.Password);
            _db.Users.Add(new_user);
            await _db.SaveChangesAsync();
            return new_user;
        }
    }
}
