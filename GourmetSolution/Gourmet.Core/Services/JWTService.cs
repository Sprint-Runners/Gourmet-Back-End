using Gourmet.Core.Domain.Entities;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.Exceptions;
using Gourmet.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gourmet.Core.Services
{
    public class JWTService : IJwt
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Chef> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JsonSerializerOptions _jsonOptions;
        public JWTService(UserManager<Chef> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jsonOptions = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
        }
        /// <summary>
        /// Provides the jwt token creation service by using token's guid,user's id,
        /// iat and user's email as its payload 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthenticationResponse CreateJwtToken(IdentityUser new_User)
        {
            DateTime Expiration = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration["JWT:Expiration_Time"]));
            List<Claim> claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,new_User.Id.ToString()),
                //new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Now).ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.UniqueName,new_User.UserName)
            };
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Mock-Key"]));
            SigningCredentials signingcredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokengenerator = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                claims: claims,
                expires: Expiration,
                signingCredentials: signingcredentials);
            JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
            string token = tokenhandler.WriteToken(tokengenerator);
            return new AuthenticationResponse()
            {
                Email = new_User.UserName,
                JWT_Token = token,
                Expiration = Expiration,
                Period = _configuration["JWT:Expiration_Time"]
            };
        }

        public bool Token_Validation(string token)
        {
            //Parsing the input token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken tokengenerator = tokenHandler.ReadJwtToken(token);
            IEnumerable<Claim> request_claims = tokengenerator.Claims;
            DateTime request_expiration = tokengenerator.ValidTo;
            string request_issuer = tokengenerator.Issuer;



            // Check the expiration date of the token
            if (DateTime.Compare(request_expiration, DateTime.UtcNow) <= 0)
                throw new InvalidTokenException("The token has expired");



            //Generate a new token to see if the token is valid
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["JWT:Mock-Key"]));
            SigningCredentials signingcredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken testtokengenerator = new JwtSecurityToken(
                request_issuer,
                claims: request_claims,
                expires: request_expiration,
                signingCredentials: signingcredentials);
            string testtoken = tokenHandler.WriteToken(testtokengenerator);
            if (token == testtoken)
                return true;
            else
                return false;


        }
        public string DecodeToken(string jwtToken)
        {
            jwtToken = jwtToken.Replace("Bearer ", "");
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(jwtToken);
            var username = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type.ToLower() == "unique_name")?.Value;
            return username;
        }
    }
}