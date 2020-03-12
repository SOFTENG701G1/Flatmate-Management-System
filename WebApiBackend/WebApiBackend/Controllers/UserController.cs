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
    public class UserController
    {
        private readonly FlatManagementContext _context;
        private readonly PasswordHasher<User> _hasher;
        private readonly AppSettings _appSettings;

        public UserController(FlatManagementContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _hasher = new PasswordHasher<User>();
        }

        [HttpPost("login")]
        public ActionResult<LoggedInDto> Login(LoginDto login)
        {
            User user = _context.User.FirstOrDefault(u => u.UserName.ToLower() == login.Username.ToLower());

            if (user == null)
            {
                return new NotFoundResult();
            }

            if (_hasher.VerifyHashedPassword(user, user.Password, login.Password) != PasswordVerificationResult.Success)
            {
                return new ForbidResult();
            }

            return new LoggedInDto(user, CreateToken(user.UserName));
        }

        [HttpPost("register")]
        public ActionResult<RegisterResponseDTO> Register(RegisterRequestDTO registerRequest)
        {  
            if (string.IsNullOrEmpty(registerRequest.UserName) || string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return new BadRequestResult();
            }

            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                UserName = registerRequest.UserName,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                DateOfBirth = registerRequest.DateOfBirth,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.Email,
                MedicalInformation = registerRequest.MedicalInformation,
                BankAccount = registerRequest.BankAccount
            };

            var hashedPassword = hasher.HashPassword(user, "password");

            user.Password = hashedPassword;

            _context.Add(user);
            _context.SaveChanges();

            return new RegisterResponseDTO
            {
                UserName = user.UserName,
                Token = CreateToken(user.UserName)
            };
        }

        private string CreateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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
