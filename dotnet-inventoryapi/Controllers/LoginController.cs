using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using dotnet_inventoryapi.Models;
using dotnet_inventoryapi.Models.utils;
using dotnet_inventoryapi.DBcontext;
using Newtonsoft.Json.Linq;

namespace dotnet_inventoryapi.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly MongoDBContext _mongoDBContext;
        private readonly IConfiguration _configuration;

        public LoginController(MongoDBContext mongoDBContext, IConfiguration configuration)
        {
            _mongoDBContext = mongoDBContext;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _mongoDBContext.Users.Find(u => u.Email == loginRequest.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return Problem("User is not found."); // User not found
            }

            var result = PasswordHasher.VerifyPassword(loginRequest.Password, user.Password);

            if (result)
            {
                // Authentication successful, generate and return a JWT
                var token = GenerateJwt(user);

                return Ok(new { Token = token });
            }

            // Authentication failed
            return Unauthorized();
        }

        private string GenerateJwt(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
