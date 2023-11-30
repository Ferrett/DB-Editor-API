using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Models.ServiceModels;
using WebAPI.Services.S3Bucket.User;
using WebAPI.Services.Validation.UserValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbcontext;
        private readonly IUserValidation userValidation;
        private readonly IUserAuthenticationService authentication;
        public AuthenticationController(IConfiguration _configuration, IUserValidation _userValidation, ApplicationDbContext _dbcontext, IUserAuthenticationService _authentication)
        {
            configuration = _configuration;
            dbcontext = _dbcontext;
            userValidation = _userValidation;
            authentication = _authentication;
        }

        [HttpPost("RegisterNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegistrationModel userRegister)
        {
            try
            {
                User newUser = authentication.RegistrationModelToUser(userRegister);

                userValidation.Validate(newUser, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await dbcontext.User.AddAsync(newUser);
                await dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(RegisterNewUser), new { id = newUser.ID }, newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginModel userLogin)
        {
            try
            {
                User? user = await authentication.FindUserByLogin(userLogin.Login);

                if (user == null)
                    return Unauthorized(new { Error = "Incorrect login" });

                if (authentication.IsPasswordCorrect(user, userLogin.Password))
                {
                    var token = GenerateJwtToken(user.Login);
                    return Ok(new { Token = token });
                }

                return Unauthorized(new { Error = "Incorrect password" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(string login)
        {
            List <Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token).ToString();

            return tokenString;
        }
    }
}
