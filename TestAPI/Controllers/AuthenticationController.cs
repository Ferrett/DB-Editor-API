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
using WebAPI.Services.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IUserValidation _userValidation;
        private readonly IUserAuthenticationService _authentication;
        public AuthenticationController(IConfiguration configuration, IUserValidation userValidation, ApplicationDbContext dbcontext, IUserAuthenticationService authentication)
        {
            _configuration = configuration;
            _dbcontext = dbcontext;
            _userValidation = userValidation;
            _authentication = authentication;
        }

        [HttpPost("RegisterNewUser")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegistrationModel userRegister)
        {
            try
            {
                User newUser = _authentication.RegistrationModelToUser(userRegister);

                _userValidation.Validate(newUser, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _dbcontext.User.AddAsync(newUser);
                await _dbcontext.SaveChangesAsync();

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
                _authentication.LoginAttempt(userLogin, ModelState);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return  Ok(new { Token = GenerateJwtToken(userLogin.Login) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(string login)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token).ToString();

            return tokenString;
        }
    }
}
