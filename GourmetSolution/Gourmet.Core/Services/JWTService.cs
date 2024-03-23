using Gourmet.Core.DTO.Request;
using Gourmet.Core.DTO.Response;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Gourmet.Core.Services
{
    public class JWTService : IJwt
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public JWTService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,new_User.UserName)
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
                Period=_configuration["JWT:Expiration_Time"]
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
    }
}