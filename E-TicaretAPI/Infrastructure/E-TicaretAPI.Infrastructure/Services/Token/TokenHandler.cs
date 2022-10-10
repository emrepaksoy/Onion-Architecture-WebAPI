using E_TicaretAPI.Application.Abstraction.Token;
using E_TicaretAPI.Domain.Entities.Identity;
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

namespace E_TicaretAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccesToken(int minute, AppUser user)
        {
            Application.DTOs.Token token = new ();

            // Security key in simetriği alınır
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimlik oluşturulur.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Oluşturulacak token ayarları yapılır.
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
            JwtSecurityToken securityToken = new
                (
                 audience: _configuration["Token:Audience"],
                 issuer: _configuration["Token:Issuer"],
                 expires: token.Expiration,
                 notBefore: DateTime.UtcNow, // bu token üretildiğin anda devreye girer. .AddMinute(1)-> 1 dk sonra devreye girer 5dk nın 1 dk sı burda geçmiş olur.
                 signingCredentials: signingCredentials
                 //claims: new List<Claim> { new(ClaimTypes.Name, user.UserName)}
                );

            // Token oluşturucu sınıfından bir örnek oluşturulur.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            //string refreshToken = CreateRefreshToken();

            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
