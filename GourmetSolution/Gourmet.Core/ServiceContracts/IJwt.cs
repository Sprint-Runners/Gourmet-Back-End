using System;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.DTO.Request;
namespace Gourmet.Core.ServiceContracts
{
    public interface IJwt
    {
        AuthenticationResponse CreateJwtToken(SignUpRequest request);
        bool Token_Validation(string token);
    }
}
