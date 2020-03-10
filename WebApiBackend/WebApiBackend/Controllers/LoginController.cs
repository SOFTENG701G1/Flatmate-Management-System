using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiBackend.Dto;
using WebApiBackend.Model;
using WebApiBackend.Helpers;
using Microsoft.Extensions.Options;
using System.Linq;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController
    {
        private readonly FlatManagementContext _context;
        private readonly PasswordHasher<User> _hasher;
        private readonly AppSettings _appSettings;

        public LoginController(FlatManagementContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _hasher = new PasswordHasher<User>();
        }

        [HttpPost]
        public ActionResult<LoggedInDto> Login(LoginDto login)
        {
            User user = _context.User.FirstOrDefault(u => u.UserName.ToLower() == login.Username.ToLower());

            if (user == null)
            {
                return new ForbidResult();
            }

            if (_hasher.VerifyHashedPassword(user, user.Password, login.Password) != PasswordVerificationResult.Success)
            {
                return new ForbidResult();
            }

            // Issue JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoggedInDto(user, tokenHandler.WriteToken(token));
        }

        protected bool AddUser(string username, string email, string password)
        {
            if (_context.User.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower()) != null)
            {
                return false;
            }

            if (_context.User.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()) != null)
            {
                return false;
            }

            var user = new User
            {
                UserName = username,
                Email = email
            };

            var hashed = _hasher.HashPassword(user, password);

            user.Password = hashed;

            _context.User.Add(user);
            _context.SaveChanges();

            return true;
        }
    }
}
