using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Helper
{
    public static class JwtTokenGeneration
    {
        public static ApplicationUser GenerateToken(ApplicationUser user, IList<string> roles = null, IList<Claim> claims=null)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Secret"];
            var key = Encoding.ASCII.GetBytes(secret);
            try
            {
                // Get valid claims and pass them into JWT
                // authentication successful so generate jwt token
                var _claims = new[]
                {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                            //new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(dateTime).ToString(), ClaimValueTypes.Integer64)
                        };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(_claims, "Token");
                // Adding roles code
                // Roles property is string collection but you can modify Select code if it it's not
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                claimsIdentity.AddClaims(claims);

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                    issuer: null,//_jwtOptions.Issuer,
                    audience: null,//_jwtOptions.Audience,
                    claims: claimsIdentity.Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256));
                user.token = tokenHandler.WriteToken(jwt);
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
