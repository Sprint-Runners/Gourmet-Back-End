using System;
using Gourmet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Core.DTO.Response
{
    public class SignUpResponse
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
    }
    public static class UsersExtentions
    {
        public static SignUpResponse ToSignUpResponse(this User user)
        {
            return new SignUpResponse() {Id=user.Id,Email=user.Email };
        }
    }
}
