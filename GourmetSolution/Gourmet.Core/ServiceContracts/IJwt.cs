using System;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.DTO.Request;
using Gourmet.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Gourmet.Core.ServiceContracts
{
    public interface IJwt
    {
        //AuthenticationResponse CreateJwtToken(IdentityUser new_user);
        string CreateJwtToken(IdentityUser new_User, List<Claim> input_claims);
        bool Token_Validation(string token);
        string DecodeToken(string jwtToken);
    }
}