using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.Helpers;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{
    class UserTest
    {
        private DatabaseSetUpHelper _dbSetUpHelper;
        private ServiceDependencyResolver _serviceProvider;
        private FlatManagementContext _context;
        private HttpContextHelper _httpContextHelper;

        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _dbSetUpHelper = new DatabaseSetUpHelper();
            _serviceProvider = _dbSetUpHelper.GetServiceDependencyResolver();
            _context = _dbSetUpHelper.GetContext();

            _httpContextHelper = new HttpContextHelper();
            var httpContext = _httpContextHelper.GetHttpContext();
            var objClaim = _httpContextHelper.GetClaimsIdentity();

            _userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            httpContext.User = new ClaimsPrincipal(objClaim);

        }

        /// <summary>
        /// Ensures a user with the correct credentials is able to login and a jwt token is returned
        /// </summary>
        [Test]
        public void TestSuccessfulLogin()
        {
            // Arrange
            var login = new LoginDto
            {
                UserName = "BeboBryan",
                Password = "password"
            };

            // Act
            var response = _userController.Login(login);

            // Assert
            Assert.IsNotNull(response.Value);
            Assert.IsFalse(string.IsNullOrEmpty(response.Value.UserName));
            Assert.IsFalse(string.IsNullOrEmpty(response.Value.Token));
        }

        /// <summary>
        /// Ensures a user who does not exist is not able to login. No JWT token is returned as they are not authenticated.
        /// </summary>
        [Test]
        public void TestIncorrectUsernameLogin()
        {
            // Arrange
            var login = new LoginDto
            {
                UserName = "UserDoesNotExist",
                Password = "password"
            };

            // Act
            var response = _userController.Login(login);

            // Assert
            Assert.IsNull(response.Value);
        }

        /// <summary>
        /// Ensures a user with incorrect password is not able to login. No JWT token is returned as they are not authenticated.
        /// </summary>
        [Test]
        public void TestIncorrectPasswordLogin()
        {
            // Arrange
            var login = new LoginDto
            {
                UserName = "user",
                Password = "IncorrectPassword"
            };

            // Act
            var response = _userController.Login(login);

            // Assert
            Assert.IsNull(response.Value);
        }

        /// <summary>
        /// Tests that a user is able to register for an account when the correct details are provided (username, password, email and 
        /// phonenumber are not empty/null and username, email and phonenumber are unique in the current database context). Ensures newly 
        /// created user is able to login.
        /// </summary>
        [Test]
        public void TestRegistration()
        {
            // Arrange
            var login = new LoginDto
            {
                UserName = "newUser",
                Password = "newUserPassword"
            };

            var registrationRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            var newLogin = new LoginDto
            {
                UserName = "newUser",
                Password = "newUserPassword"
            };

            // Act
            var initialLoginResponse = _userController.Login(login);

            // Assert
            Assert.IsNull(initialLoginResponse.Value);

            // Act
            var registrationResponse = _userController.Register(registrationRequest);

            // Assert
            Assert.IsNotNull(registrationResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationResponse.Value.Token));

            // Act
            ActionResult<LoggedInDto> finalLoginResponse = _userController.Login(newLogin);

            // Assert
            Assert.IsNotNull(finalLoginResponse.Value);
            Assert.IsFalse(string.IsNullOrEmpty(finalLoginResponse.Value.UserName));
            Assert.IsFalse(string.IsNullOrEmpty(finalLoginResponse.Value.Token));

            Assert.AreEqual(finalLoginResponse.Value.UserName, registrationResponse.Value.UserName);
        }

        /// <summary>
        /// Checks that UserController does not allow multiple users with the same username to be created. The second user registration will fail
        /// </summary>
        [Test]
        public void TestRegistrationWithDuplicateUserName()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            var dupRegReq = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "1234f59",
                Email = "newuser2@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            // Act
            var registrationOneResponse = _userController.Register(registerRequest);

            // Assert
            Assert.IsNotNull(registrationOneResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.Token));

            // Act
            var registrationTwoResponse = _userController.Register(dupRegReq);

            // Assert
            Assert.That(registrationTwoResponse.Result, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow multiple users with the same email to be created. The second user registration will fail
        /// </summary>
        [Test]
        public void TestRegistrationWithDuplicateEmail()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            var duplicateRegisterRequest = new RegisterRequestDTO
            {
                UserName = "newUser2",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "1234f59",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            // Act
            var registrationOneResponse = _userController.Register(registerRequest);

            // Assert
            Assert.IsNotNull(registrationOneResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.Token));

            // Act
            var registrationTwoResponse = _userController.Register(duplicateRegisterRequest);

            // Assert
            Assert.That(registrationTwoResponse.Result, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow multiple users with the same phonenumber to be created. The second user registration will fail
        /// </summary>
        [Test]
        public void TestRegistrationWithDuplicatePhoneNumber()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            var duplicateRegisterRequest = new RegisterRequestDTO
            {
                UserName = "newUser2",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser2@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            // Act
            var registrationOneResponse = _userController.Register(registerRequest);

            // Assert
            Assert.IsNotNull(registrationOneResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.Token));

            // Act
            var registrationTwoResponse = _userController.Register(duplicateRegisterRequest);

            // Assert
            Assert.That(registrationTwoResponse.Result, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController allows a valid username & email combination
        /// </summary>
        [Test]
        public void TestCheckWithValidUserAndEmail()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                UserName = "user1",
                Email = "email@test.co.nz"
            };

            // Act
            ActionResult checkUserResponse = _userController.CheckUser(registerRequest);

            // Assert
            Assert.That(checkUserResponse, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow a duplicate username
        /// </summary>
        [Test]
        public void TestCheckWithDuplicateUser()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            var duplicateRegisterRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                Email = "email@test.co.nz"
            };

            // Act
            _ = _userController.Register(registerRequest);
            var checkUserResponse = _userController.CheckUser(duplicateRegisterRequest);

            // Assert
            Assert.That(checkUserResponse, Is.InstanceOf<BadRequestResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow a duplicate email
        /// </summary>
        [Test]
        public void TestCheckWithDuplicateEmail()
        {
            // Arrange
            var registerRequest = new RegisterRequestDTO
            {
                UserName = "newUser",
                FirstName = "New",
                LastName = "User",
                DateOfBirth = new DateTime(),
                PhoneNumber = "123459",
                Email = "newuser@test.co.nz",
                MedicalInformation = "N/A",
                BankAccount = "84903",
                Password = "newUserPassword"
            };

            var duplicateRegisterRequest = new RegisterRequestDTO
            {
                UserName = "User1",
                Email = "newUser@test.co.nz"
            };

            // Act
            _ = _userController.Register(registerRequest);
            var checkUserResponse = _userController.CheckUser(duplicateRegisterRequest);

            // Assert
            Assert.That(checkUserResponse, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController correctly retrives user's id
        /// </summary>
        [Test]
        public void TestCheckCurrentUserID()
        {
            // Arrange
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "3") });
            _userController.HttpContext.User = new ClaimsPrincipal(objClaim);

            // The ID of the currently authorised user can be found and modified in Helper/HttpConextHelper
            var response = _userController.GetCurrentUserID().Value;
            Assert.AreEqual(3, response);
        }

        /// <summary>
        /// Checks that UserController correctly retrives user's id
        /// </summary>
        [Test]
        public void TestGetUsersIdsByUsernames()
        {
            UserIdDTO userIds = new UserIdDTO();
            userIds.UserID = new Dictionary<string, string>();
            userIds.UserID.Add("TestUser1", "0");
            userIds.UserID.Add("TestUser2", "0");
            userIds.UserID.Add("Non-existing user", "0");

            // The ID of the currently authorised user can be found and modified in Helper/HttpConextHelper
            userIds = _userController.GetUsersIdsByUsernames(userIds).Result;
            Assert.AreEqual("998", userIds.UserID["TestUser1"]);
            Assert.AreEqual("999", userIds.UserID["TestUser2"]);
            Assert.AreEqual("0", userIds.UserID["Non-existing user"]);

        }
        /// <summary>
        /// Checks that UserController returns the correct user information
        /// </summary>
        [Test]
        public void TestGetUserInfo()
        {
            //Arrange
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "3") });
            _userController.HttpContext.User = new ClaimsPrincipal(objClaim);
            

            //Act
            var response = _userController.GetUserInfo();

            //Assert
            Assert.IsInstanceOf<UserInfoDTO>(response.Value);
            Assert.That(response.Value.Id == 3);
            Assert.That(response.Value.UserName == "BeboBryan");
            Assert.That(response.Value.FirstName == "Bryan");
            Assert.That(response.Value.LastName == "Ang");
            Assert.That(response.Value.DateOfBirth == new DateTime(1984, 02, 09));
            Assert.That(response.Value.PhoneNumber == "02243926392");
            Assert.That(response.Value.Email == "BryanAng@Gmail.com");
            Assert.That(response.Value.MedicalInformation == "N/A");
            Assert.That(response.Value.BankAccount == "98-7654-3211234-210");
        }

        [Test]
        /// <summary>
        /// Checks that EditUSer info seuccesfully edits a users info
        /// </summary>
        public void TestEditUserInfo()
        {
            //Arrange
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "3") });
            _userController.HttpContext.User = new ClaimsPrincipal(objClaim);
            EditUserDTO editUser = new EditUserDTO();
            editUser.FirstName = "Bryan_Test";
            editUser.LastName = "Ang_Test";
            editUser.DateOfBirth = new DateTime(1984, 02, 09);
            editUser.PhoneNumber = "5555555555";
            editUser.Email = "edittest@edittest.com";
            editUser.MedicalInformation = "editing test";
            editUser.BankAccount = "111-1111-1111111";

            //Act
            _ = _userController.EditUserInfo(editUser);
            var response = _userController.GetUserInfo();
            UserInfoDTO editedUser = response.Value;

            //Assert
            Assert.That(editedUser.FirstName == editUser.FirstName);
            Assert.That(editedUser.LastName == editUser.LastName);
            Assert.That(editedUser.DateOfBirth == editUser.DateOfBirth);
            Assert.That(editedUser.PhoneNumber == editUser.PhoneNumber);
            Assert.That(editedUser.Email == editUser.Email);
            Assert.That(editedUser.MedicalInformation == editUser.MedicalInformation);
            Assert.That(editedUser.BankAccount == editUser.BankAccount);
        }

    }
}
