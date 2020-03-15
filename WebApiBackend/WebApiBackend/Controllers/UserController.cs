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
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly FlatManagementContext _database;

        public UserController( IOptions<AppSettings> appSettings, FlatManagementContext context)
        {
            _appSettings = appSettings.Value;
            _database = context;
        }

        [HttpPost("login")]
        public ActionResult<LoggedInDto> Login(LoginDto login)
        {
            User user = _database.User.FirstOrDefault(u => u.UserName.ToLower() == login.Username.ToLower());

            if (user == null)
            {
                return new NotFoundResult();
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();

            if (hasher.VerifyHashedPassword(user, user.HashedPassword, login.Password) != PasswordVerificationResult.Success)
            {
                return new ForbidResult();
            }

            return new LoggedInDto(user, CreateToken(user.UserName));
        }

        [HttpPost("register")]
        public ActionResult<RegisterResponseDTO> Register(RegisterRequestDTO registerRequest)
        {
            // Checking if the non nullable fields (as per business rules) are not empty/null 
            if (string.IsNullOrEmpty(registerRequest.UserName) || string.IsNullOrEmpty(registerRequest.Email) || 
                string.IsNullOrEmpty(registerRequest.PhoneNumber) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return new BadRequestResult();
            }

            User user;
            if (TryCreateUser(registerRequest.UserName, registerRequest.FirstName, registerRequest.LastName, registerRequest.DateOfBirth, registerRequest.PhoneNumber, 
                registerRequest.Email, registerRequest.MedicalInformation, registerRequest.BankAccount, registerRequest.Password, out user))
            {
                return new RegisterResponseDTO
                {
                    UserName = user.UserName,
                    Token = CreateToken(user.UserName)
                };
            }
            else
            {
                return new ConflictResult();
            }
        }

        private string CreateToken(string username)
        {
            // Creates jwt token for user based on user's username as username is the primary key of the user.
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

        private bool TryCreateUser(string userName, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, 
            string email, string medicalInformation, string bankAccount, string password, out User user)
        {
            user = null;

            // Returns false if username is not unique (must be unique as per entity schema). Does not create user.
            if (_database.User.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower()) != null)
            {
                return false; 
            }

            // Returns false if email is not unique (must be unique as per entity schema). Does not create user.
            if (_database.User.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()) != null)
            {
                return false;
            }

            // Returns false if phone number is not unique (must be unique as per entity schema). Does not create user.
            if (_database.User.FirstOrDefault(u => u.PhoneNumber.ToLower() == phoneNumber.ToLower()) != null)
            {
                return false;
            }

            user = new User
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                PhoneNumber = phoneNumber,
                Email = email,
                MedicalInformation = medicalInformation,
                BankAccount = bankAccount
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, password);
            user.HashedPassword = hashedPassword;

            _database.Add(user);
            _database.SaveChanges();

            return true;
        }

        
    }
}
