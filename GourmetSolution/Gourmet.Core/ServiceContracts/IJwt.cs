using System;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Gourmet.Core.ServiceContracts
{
    public interface IJwt
    {
        AuthenticationResponse CreateJwtToken(IdentityUser new_user);
        bool Token_Validation(string token);
        string DecodeToken(string jwtToken);
    }
}