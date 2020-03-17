using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
using System.Text.RegularExpressions;
using System.Security.Policy;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
//using System.Web.Mvc;

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
            return new LoggedInDto(user, CreateToken(user.UserName));
        }

        [HttpPost("resetTokenCheck")]
        public ActionResult<ResetConfirmedDTO> CheckResetToken(ResetPasswordDTO resetPassword)
        {
            if (string.IsNullOrEmpty(resetPassword.Email) && string.IsNullOrEmpty(resetPassword.ResetToken))
            {
                return new OkResult();
            }

            // Email and Token always come in pairs. If one is present while the other is missing, that means someone is sending false input
            if (string.IsNullOrEmpty(resetPassword.Email) && !string.IsNullOrEmpty(resetPassword.ResetToken))
            {
                return new BadRequestResult();
            }

            if (!string.IsNullOrEmpty(resetPassword.Email) && string.IsNullOrEmpty(resetPassword.ResetToken))
            {
                return new BadRequestResult();
            }

            // Find if the user exists in the DB
            var findUser = _database.User.FirstOrDefault(u => u.Email.ToLower() == resetPassword.Email.ToLower());
            if (findUser != null)
            {
                // If so, check if it has been used and if it has expired and have a token assigned
                if (findUser.ResetToken == resetPassword.ResetToken && findUser.HaveReset == false)
                {
                    if (!CheckExpired(resetPassword.ResetToken))
                    { 
                        findUser.HaveReset = true;
                        _database.SaveChanges();
                        return new ResetConfirmedDTO
                        {
                            Email = resetPassword.Email
                        };
                    }
                }
            }
            return new BadRequestResult();
        }
        
        [HttpPost("resetPassword")]
        public ActionResult ResetPassWord(LoginDto login)
        {
            User user = _database.User.FirstOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower());

            if (user == null)
            {
                return new NotFoundResult();
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, login.Password);
            user.HashedPassword = hashedPassword;
            _database.Add(user);
            _database.SaveChanges();

            return new OkResult();
        }

        [HttpPost("forgotPassword")]
        public ActionResult<ForgotPassResponseDTO> ForgotPassword(ForgotPassRequestDTO forgotPass)
        {
            
            // Checking if the non nullable fields (as per business rules) are not empty/null 
            if (string.IsNullOrEmpty(forgotPass.userOrEmail))
            {
                return new NotFoundResult();
            }

            // Check if user input their username or password
            // using Regex to check if it's email, 
            // if email, Email = u.userOrEmail
            string email_tmp = "";
            string user_tmp;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(forgotPass.userOrEmail);
            if (match.Success){
                email_tmp = forgotPass.userOrEmail;
                // Check if email exists in the DB, if not then it's a bad request
                if (_database.User.FirstOrDefault(u => u.Email.ToLower() == email_tmp.ToLower()) == null)
                {
                    return new NotFoundResult();
                }
            }
            else
            {
                user_tmp = forgotPass.userOrEmail;
                // Check if user exists in the DB, if not then it's a bad request
                if (_database.User.FirstOrDefault(u => u.UserName.ToLower() == user_tmp.ToLower()) != null)
                {
                    // TODO Get the email by querying username and set email_tmp to the response
                    var findUser = _database.User.FirstOrDefault(u => u.UserName.ToLower() == user_tmp.ToLower());
                    findUser.Email = email_tmp;
                    _database.SaveChanges();

                }
                else
                {
                    return new NotFoundResult();
                }
            }

            // Generate token
            string token_tmp = GenResetToke();

            // Combine to make a link with the email
            MailMessage mailMessage = new MailMessage("se701uoa2020@gmail.com", email_tmp);
            mailMessage.Body = "http://localhost:3000/login/reset-password?email=" + email_tmp + "&token=" + token_tmp;
            mailMessage.Subject = "Reset your password";

            // Specify the SMTP server name and post number speciic to gmail
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "se701uoa2020@gmail.com",
                Password = "flatmate2020!"
            };
            
            // Gmail works on SSL, so set this property needs to be true
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

            // Store the token into DB and toggle the token check
            var userReset = _database.User.FirstOrDefault(u => u.Email == email_tmp);
            if (userReset != null)
            {
                userReset.ResetToken = token_tmp;
                userReset.HaveReset = false;
                _database.SaveChanges();
            }

            return new ForgotPassResponseDTO
            {
                Email = email_tmp,
                ResetToken = token_tmp
            };

        }

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

        private string GenResetToke()
        {
            // Generate token that contains a timestamp
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        private Boolean CheckExpired(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-1))
            {
                // Return true if token has expired
                return true;
            }
            return false;
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
                BankAccount = bankAccount,
                ResetToken = "",
                HaveReset = true,
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
