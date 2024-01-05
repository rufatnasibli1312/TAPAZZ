using Entity.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceExtensions
{
    public class JwtTokenExtractor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenExtractor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetFullnameFromJwtToken()
        {
            var handler = new JwtSecurityTokenHandler();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            jwtToken = jwtToken.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var fullNameClaim = jsonToken?.Claims?.FirstOrDefault(claim => claim.Type == "Fullname");
            var fullname = fullNameClaim.Value;
            return fullname;
        }
        public string GetUserIdFromJwtToken()
        {
            var handler = new JwtSecurityTokenHandler();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            jwtToken = jwtToken.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
            var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "Id");
            var userId = userIdClaim.Value;

            return userId;
        }
    }
}
