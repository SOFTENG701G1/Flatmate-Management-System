using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
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

        [SetUp]
        public void Setup()
        {
            _dbSetUpHelper = new DatabaseSetUpHelper();
            _serviceProvider = _dbSetUpHelper.GetServiceDependencyResolver();
            _context = _dbSetUpHelper.GetContext();
        }

        /// <summary>
        /// Ensures a user with the correct credentials is able to login and a jwt token is returned
        /// </summary>
        [Test]
        public void TestSuccessfulLogin()
        {
            UserController userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<LoggedInDto> response = userController.Login(new LoginDto
            {
                UserName = "BeboBryan",
                Password = "password"
            });

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
            UserController userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<LoggedInDto> response = userController.Login(new LoginDto
            {
                UserName = "UserDoesNotExist",
                Password = "password"
            });

            Assert.IsNull(response.Value);
        }

        /// <summary>
        /// Ensures a user with incorrect password is not able to login. No JWT token is returned as they are not authenticated.
        /// </summary>
        [Test]
        public void TestIncorrectPasswordLogin()
        {
            UserController userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<LoggedInDto> response = userController.Login(new LoginDto
            {
                UserName = "user",
                Password = "IncorrectPassword"
            });

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
            UserController initialLoginUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<LoggedInDto> initialLoginResponse = initialLoginUserController.Login(new LoginDto
            {
                UserName = "newUser",
                Password = "newUserPassword"
            });

            Assert.IsNull(initialLoginResponse.Value);

            UserController registrationUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationResponse = registrationUserController.Register(new RegisterRequestDTO
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
            });

            Assert.IsNotNull(registrationResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationResponse.Value.Token));

            UserController finalLoginUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<LoggedInDto> finalLoginResponse = finalLoginUserController.Login(new LoginDto
            {
                UserName = "newUser",
                Password = "newUserPassword"
            });

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
            UserController registrationOneUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationOneResponse = registrationOneUserController.Register(new RegisterRequestDTO
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
            });

            Assert.IsNotNull(registrationOneResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.Token));

            UserController registrationTwoUserController =  new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationTwoResponse = registrationTwoUserController.Register(new RegisterRequestDTO
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
            });

            Assert.That(registrationTwoResponse.Result, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow multiple users with the same email to be created. The second user registration will fail
        /// </summary>
        [Test]
        public void TestRegistrationWithDuplicateEmail()
        {
            UserController registrationOneUserController =  new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationOneResponse = registrationOneUserController.Register(new RegisterRequestDTO
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
            });

            Assert.IsNotNull(registrationOneResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.Token));

            UserController registrationTwoUserController =  new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationTwoResponse = registrationTwoUserController.Register(new RegisterRequestDTO
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
            });

            Assert.That(registrationTwoResponse.Result, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow multiple users with the same phonenumber to be created. The second user registration will fail
        /// </summary>
        [Test]
        public void TestRegistrationWithDuplicatePhoneNumber()
        {
            UserController registrationOneUserController =  new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationOneResponse = registrationOneUserController.Register(new RegisterRequestDTO
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
            });

            Assert.IsNotNull(registrationOneResponse.Value);
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.UserName));
            Assert.False(string.IsNullOrEmpty(registrationOneResponse.Value.Token));

            UserController registrationTwoUserController =  new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationTwoResponse = registrationTwoUserController.Register(new RegisterRequestDTO
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
            });

            Assert.That(registrationTwoResponse.Result, Is.InstanceOf<ConflictResult>());
        }

        /// <summary>
        /// Checks that UserController allows a valid username & email combination
        /// </summary>
        [Test]
        public void TestCheckWithValidUserAndEmail()
        {
            UserController checkUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult checkUserResponse = checkUserController.CheckUser(new RegisterRequestDTO
            {
                UserName = "user1",
                Email = "email@test.co.nz"
            });

            Assert.That(checkUserResponse, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow a duplicate username
        /// </summary>
        [Test]
        public void TestCheckWithDuplicateUser()
        {
            UserController registrationUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationResponse = registrationUserController.Register(new RegisterRequestDTO
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
            });

            UserController checkUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult checkUserResponse = checkUserController.CheckUser(new RegisterRequestDTO
            {
                UserName = "newUser",
                Email = "email@test.co.nz"
            });

            Assert.That(checkUserResponse, Is.InstanceOf<BadRequestResult>());
        }

        /// <summary>
        /// Checks that UserController does not allow a duplicate email
        /// </summary>
        [Test]
        public void TestCheckWithDuplicateEmail()
        {
            UserController registrationUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult<RegisterResponseDTO> registrationResponse = registrationUserController.Register(new RegisterRequestDTO
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
            });

            UserController checkUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);
            ActionResult checkUserResponse = checkUserController.CheckUser(new RegisterRequestDTO
            {
                UserName = "User1",
                Email = "newUser@test.co.nz"
            });

            Assert.That(checkUserResponse, Is.InstanceOf<ConflictResult>());
        }
    }
}
