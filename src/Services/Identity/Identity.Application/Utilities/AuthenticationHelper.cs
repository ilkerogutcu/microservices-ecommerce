using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Utilities
{
    public static class AuthenticationHelper
    {
        public static async Task<JwtSecurityToken> GenerateJwtToken(User user, IConfiguration configuration,
            UserManager<User> userManager, string ipAddress)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(t => new Claim("roles", t)).ToList();
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sid, user.SecurityStamp),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, "https://github.com/ilkerogutcu/.Net-Core-5-boilerplate"),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                    new Claim("ip", ipAddress)
                }
                .Union(userClaims)
                .Union(roleClaims);
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenOptions:SecurityKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            return new JwtSecurityToken(
                configuration["TokenOptions:Issuer"],
                configuration["TokenOptions:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["TokenOptions:DurationInMinutes"])),
                signingCredentials: signingCredentials);
        }
    }
}