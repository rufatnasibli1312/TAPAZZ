using Azure;
using DTO.JWTDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Http;
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

namespace BLL.JWT
{
    public static class JwtHelper
    {

        public static ResponseDto GenerateToken(IConfiguration _configuration, List<Claim> claims)
        {
            var result = new ResponseDto();

            var key = Encoding.UTF8.GetBytes(_configuration["JWTToken:Key"]);
            var audience = _configuration["JWTToken:Audience"];
            var issuer = _configuration["JWTToken:Issuer"];
            var signInCredential = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims : claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signInCredential

            );



            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            result.AccessToken = token;
            result.RefreshToken = refreshToken;


            return result;

        }


        public static string GenerateRefreshToken()
        {

            var bytes = new byte[32];
            using (var nums = RandomNumberGenerator.Create())
            {
                nums.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

    }

}


