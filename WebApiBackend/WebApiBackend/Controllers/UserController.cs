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
        private readonly AppSettings _appSettings;
        private readonly FlatManagementContext _database;

        public UserController( IOptions<AppSettings> appSettings, FlatManagementContext context)
        {
            _appSettings = appSettings.Value;
            _database = context;
        }

        /// <summary>
        /// POST Method - Logs a user with the specified username/password combination in by returning a JWT token
        /// </summary>
        /// <param name="login">The username/password combination of a user</param>
        /// <returns>A JWT token, to be used for authentication of subsequent requests</returns>
        [HttpPost("login")]
        public ActionResult<LoggedInDto> Login(LoginDto login)
        {
            User user = _database.User.FirstOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower());

            if (user == null)
            {
                return new NotFoundResult();
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();

            if (hasher.VerifyHashedPassword(user, user.HashedPassword, login.Password) != PasswordVerificationResult.Success)
            {
                return new ForbidResult();
            }

            return new LoggedInDto(user, CreateToken(user.Id));
        }

        /// <summary>
        /// POST Method - Checks if an account with the specified username or email exists
        /// </summary>
        /// <param name="registerRequest">A RegisterRequestDTO object with username and email values set</param>
        /// <returns>An error if an account exists, else an ok message</returns>
        [HttpPost("check")]
        public ActionResult CheckUser(RegisterRequestDTO registerRequest)
        {
            // Check if username is already used (must be unique as per entity schema)
            if (_database.User.FirstOrDefault(u => u.UserName.ToLower() == registerRequest.UserName.ToLower()) != null)
            {
                return new BadRequestResult();
            }

            // Check if email is already used (must be unique as per entity schema)
            if (_database.User.FirstOrDefault(u => u.Email.ToLower() == registerRequest.Email.ToLower()) != null)
            {
                return new ConflictResult();
            }

            return new OkResult();
        }

        /// <summary>
        /// POST Method - Registers a user with the specified paramaters.
        /// </summary>
        /// <param name="registerRequest">The parameters of the user which will be set on registration</param>
        /// <returns>A JWT token if account is created, else an error</returns>
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
                    Token = CreateToken(user.Id)
                };
            }
            else
            {
                return new ConflictResult();
            }
        }

        /// <summary>
        /// Creates a JWT token for users with the specified UserID
        /// </summary>
        /// <param name="userID">The UserID of the account to provide a JWT token to</param>
        /// <returns>A JWT token</returns>
        private string CreateToken(int userID)
        {
            // Creates jwt token for user based on user's username as username is the primary key of the user.
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Tries to create a new user with the specified parameters
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="medicalInformation"></param>
        /// <param name="bankAccount"></param>
        /// <param name="password"></param>
        /// <param name="user">A user object - populated if the creation was successful</param>
        /// <returns>A bool indicating if a user was created</returns>
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
                BankAccount = bankAccount,
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
