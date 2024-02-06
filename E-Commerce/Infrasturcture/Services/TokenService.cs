using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
            key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
        }
        public string CreateToken(AppUser user)
        {
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.DisplayName)
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject= new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(10),
                SigningCredentials=creds,
                Issuer = configuration["Token:Issuer"],
                IssuedAt= DateTime.Now
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
