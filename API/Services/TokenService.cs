using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config){
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            // Tokenkey được cấu hình trong file appsettings.Development.json(phải lơn hơn 512 ký tự)
        }
        public string CreateToken(AppUser user)
        {
           var claims  = new List<Claim>
           {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
           };
           var creds = new SigningCredentials(_key , SecurityAlgorithms.HmacSha512Signature);
           var tokenDescriptor = new SecurityTokenDescriptor
           {
            Subject= new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),   // hạn sử dụng của Token là 7 ngày
            SigningCredentials  = creds
           };

           // lưu Token
           var tokenHandler = new JwtSecurityTokenHandler();
           var token = tokenHandler.CreateToken(tokenDescriptor) ;
           return tokenHandler.WriteToken(token);
        }
    }
}