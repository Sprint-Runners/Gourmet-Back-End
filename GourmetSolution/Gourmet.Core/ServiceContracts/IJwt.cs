using System;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.DTO.Request;


namespace Gourmet.Core.ServiceContracts
{
    public interface IJwt
    {
        AuthenticationResponse CreateJwtToken(Login_Request request,Guid Id);
        bool Token_Validation(string token);
    }
}
