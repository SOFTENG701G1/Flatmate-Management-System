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
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Web;
using System.Collections.Generic;

namespace WebApiBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly FlatManagementContext _database;

        public UserController(IOptions<AppSettings> appSettings, FlatManagementContext context)
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
        /// POST Method - Confirm if the given reset password token for a user is valid.
        /// </summary>
        /// <param name="resetPassword">A ResetPaddwordDTO object with user E-mail and reset password token to be validated</param>
        /// <returns>An error if the token
        /// has expirese (lifespan over 1 hours,
        /// does not match with the given user,
        /// user cannot be found in the DB,
        /// else, return ok</returns>
        [HttpPost("resetTokenCheck")]
        public ActionResult CheckResetToken(ResetPasswordDTO resetPassword)
        {
            if (!string.IsNullOrEmpty(resetPassword.Email) && !string.IsNullOrEmpty(resetPassword.ResetToken))
            {
                // Find if the user exists in the DB
                var findUser = _database.User.FirstOrDefault(u => u.Email.ToLower() == resetPassword.Email.ToLower());
                if (findUser != null)
                {
                    // Check if the token has been used and if it has expired and have a token assigned
                    if (findUser.ResetToken == resetPassword.ResetToken)
                    {
                        if (!CheckExpired(resetPassword.ResetToken))
                        {
                            // Clear the reset token in DB so it can only be used once
                            findUser.ResetToken = "";
                            _database.SaveChanges();
                            return new OkResult();
                        }
                    }
                }
            }
            return new BadRequestResult();
        }
        [HttpPost("getUserInfo")]
        [Authorize]
        public ActionResult<UserInfoDTO> GetUserInfo()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _database.User.FirstOrDefault(x => x.Id == userID);
            if(user == null)
            {
                return new BadRequestResult();
            }

            return new UserInfoDTO(user);
        }


        /// <summary>
        /// POST Method - Edits user info
        /// </summary>
        /// <param name="info">An EditUserDTO object with user details to be updated</param>
        /// <returns>An error if the token
        /// has expired (lifespan over 1 hours,
        /// does not match with the given user,
        /// user cannot be found in the DB,
        /// else, return ok</returns>
        /// 
        [HttpPost("editUserInfo")]
        [Authorize]
        public ActionResult EditUserInfo(EditUserDTO info)
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (TryEditUser(userID, info.FirstName, info.LastName, info.DateOfBirth, info.PhoneNumber, info.Email, info.MedicalInformation, info.BankAccount))
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }


        }
        /// <summary>
        /// POST Method - Update the given user's password.
        /// </summary>
        /// <param name="login">The E-mail and the newly requested password.</param>
        /// <returns>An error if the account/E-mail doesn't exists or
        /// if E-mail not allowed to change password, else an ok message</returns>
        [HttpPost("resetPassword")]
        public ActionResult ResetPassWord(LoginDto login)
        {
            User user = _database.User.FirstOrDefault(u => u.Email.ToLower() == login.UserName.ToLower());

            if (user == null)
            {
                return new NotFoundResult();
            }

            // If the account did not have a valid password change request, then reject the action.
            if (!user.CanReset)
            {
                return new BadRequestResult();
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, login.Password);
            user.HashedPassword = hashedPassword;
            user.CanReset = false;
            _database.SaveChanges();
            return new OkResult();
        }

        /// <summary>
        /// POST Method - Send reset password link to user's E-mail.
        /// </summary>
        /// <param name="forgotPass">The username or email that is requesting for password reset.</param>
        /// <returns>The targeted E-mail address and base 64 string token</returns>
        [HttpPost("forgotPassword")]
        public ActionResult<ForgotPassResponseDTO> ForgotPassword(ForgotPassRequestDTO forgotPass)
        {

            // Checking if the non nullable fields (as per business rules) are not empty/null 
            if (string.IsNullOrEmpty(forgotPass.userOrEmail))
            {
                return new NotFoundResult();
            }

            // Check if user input their username or E-mail, and convert username to equivalent E-mail if inputted
            string email_tmp = "";
            string user_tmp;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(forgotPass.userOrEmail);
            if (match.Success)
            {
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
                    // Get the email by querying username and set email_tmp to the response
                    var findUser = _database.User.FirstOrDefault(u => u.UserName.ToLower() == user_tmp.ToLower());
                    email_tmp = findUser.Email;
                }
                else
                {
                    return new NotFoundResult();
                }
            }

            // Generate token
            string token_tmp = GenResetToke();

            // Send an E-mail to user with a reset password link
            SendResetEmail(email_tmp, token_tmp);

            // Store the token into DB and allow this user to reset password
            var userReset = _database.User.FirstOrDefault(u => u.Email == email_tmp);
            if (userReset != null)
            {
                userReset.ResetToken = token_tmp;
                userReset.CanReset = true;
                _database.SaveChanges();
            }

            return new ForgotPassResponseDTO
            {
                Email = email_tmp,
                ResetToken = token_tmp
            };
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
        /// Generate a base 64 string token that's generated based on time of creation
        /// </summary>
        /// <returns>A base 64 string token</returns>
        private string GenResetToke()
        {
            // Generate token that contains a timestamp
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }

        /// <summary>
        /// Check if the given token has expired (lifetime over 1 hour)
        /// </summary>
        /// <param name="token">A base 64 string token that's generated based on time of creation</param>
        /// <returns>A bool indicating if a toke has expired</returns>
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
                ResetToken = "",
                CanReset = false,
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(user, password);
            user.HashedPassword = hashedPassword;
            _database.Add(user);
            _database.SaveChanges();

            return true;
        }
        /// <summary>
        /// Tries to edit a users information
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="medicalInformation"></param>
        /// <param name="bankAccount"></param>
        /// <returns>A bool indicating if a user info was edited</returns>
        private bool TryEditUser(int userID, string firstName, string lastName, DateTime dateOfBirth, string phoneNumber, string email, string medicalInformation, string bankAccount)
        {
            var user = _database.User.FirstOrDefault(x => x.Id == userID);

            if (user == null)
            {
                return false;
            }
            // Returns false if email is in use by other user
            if (_database.User.FirstOrDefault(u => u.Email.ToLower() == email.ToLower()) != null && (email != user.Email))
            {
                return false;
            }

            // Returns false if phone number is in use by other user
            if (_database.User.FirstOrDefault(u => u.PhoneNumber.ToLower() == phoneNumber.ToLower()) != null && (email != user.Email))
            {
                return false;
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.DateOfBirth = dateOfBirth;
            user.PhoneNumber = phoneNumber;
            user.Email = email;
            user.MedicalInformation = medicalInformation;
            user.BankAccount = bankAccount;
            _database.SaveChanges();
            return true;

        }
        /// <summary>
        /// Send an E-mail to detination with a link for user to reset password.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="resetToken"></param>
        private void SendResetEmail(string destination, string resetToken)
        {
            MailMessage mailMessage = new MailMessage("se701uoa2020@gmail.com", destination);

            // Further detilas could be added so the content of the E-mail is more informative
            // Currently it's hard coding the full URL prior to the query string due to techinical difficulties
            mailMessage.Body = "http://localhost:3000/login/reset-password?email=" + destination + "&token=" + resetToken;
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
        }

        /// <summary>
        /// GET Method - Get the id of the current user
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns>Retrieves the id of the current user</returns>
        [HttpGet("getCurrentUserID")]
        [Authorize]
        public ActionResult<int> GetCurrentUserID()
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                if (userID != 0)
                {
                    return userID;
                }
                return null;
            } catch
            {
                return new UnauthorizedResult();
            }
        }

        /// <summary>
        /// GET Method - Maps usernames to their ids
        /// </summary>
        /// <param name="userIds">A UserIdDTO object with username and a default id (must be 0)</param>
        /// <returns>If a username is found, the id gets updated, otherwise it is assigned a "0" value. Returns a key/value pair of usernames and their ids </returns>
        [HttpPost("getUsersIds")]
        public UserIdDTO GetUsersIdsByUsernames(UserIdDTO userIds)
        {

            // Search for nicknames in the database, assign the found id to as a value to the dictionary
            UserIdDTO usernameIds = userIds;
            usernameIds.UserID = userIds.UserID;
            foreach(String entry in userIds.UserID.Keys.ToList())
            {
                if (_database.User.FirstOrDefault(u => u.UserName.ToLower() == entry.ToLower()) != null)
                {
                    string userId = _database.User.FirstOrDefault(u => u.UserName.ToLower().Equals(entry.ToLower())).Id.ToString();
                    usernameIds.UserID[entry] = userId;
                } else
                {
                    usernameIds.UserID[entry] = "0";
                }
            }
            return usernameIds;
        }
    }
}


































