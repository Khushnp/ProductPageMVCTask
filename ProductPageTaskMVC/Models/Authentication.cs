using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductPageTaskMVC.Models
{
    public class Authentication
    {
        public static string GenerateJetToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid. NewGuid().ToString()),
                new Claim (ClaimTypes.NameIdentifier, username)
            };
            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Convert.ToString("C1CF4B7DC4C4175B6618DE4F55CA4KJADHKAJGDAKDGKADGKA")));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var _expires = DateTime.Now.AddDays(Convert.ToDouble(Convert.ToString("30")));

            var token = new JwtSecurityToken(
                Convert.ToString("https://localhost:44318"),
                Convert.ToString("SecureAplUser"),
                claims,
                expires: _expires,
                signingCredentials : creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);            
        }

        public static string ValidateToken(string token)
        {
            if(token == null)return null;
            
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Convert.ToString("C1CF4B7DC4C4175B6618DE4F55CA4KJADHKAJGDAKDGKADGKA"));
            try
            {
                tokenhandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var jti = jwtToken.Claims.First(claim => claim.Type == "jti").Value;
                var username = jwtToken.Claims.First(sub => sub.Type == "sub").Value;
                
                //return user id from JWT token if validation successful
                return username;
            }
            catch
            {
                return null;
            }

        }
    }
}
