using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductPageMVCTask.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductPageMVCTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        //logic for getting JWT token from the private methods-->
        private string CreateJWT(UserModel user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("THIS IS THE SECRET KEY")); // NOTE: SAME KEY AS USED IN Startup.cs FILE
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name , user.username!),
                new Claim(JwtRegisteredClaimNames.Sub, user.username!),
                new Claim(JwtRegisteredClaimNames.Email, user.email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
            }; 
            var token = new JwtSecurityToken(issuer: "domain.com", audience: "domain.com", claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Admin Authentication
        private UserModel? Authenticate(UserModel login)
        {
            if (login.username == "Admin" && login.password == "admin") // NOTE: in production, query a database for user information
                return new UserModel { username = login.username, email = "Admin@gmail.com" };
            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel login)
        {
            return await Task.Run(() =>
            {
                IActionResult response = Unauthorized();

                UserModel user = Authenticate(login)!;

                if (user != null)
                    response = Ok(new { token = CreateJWT(user) });

                return response;
            });
        }

    }
}
