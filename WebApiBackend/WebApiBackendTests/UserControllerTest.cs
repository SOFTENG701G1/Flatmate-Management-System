using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using WebApiBackend;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.Helpers;
using WebApiBackend.Model;

namespace WebApiBackendTests
{
    class UserTest
    {
        ServiceDependencyResolver _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _serviceProvider = new ServiceDependencyResolver(webHost);

            FlatManagementContext context = new FlatManagementContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var testDataGenerator = new DevelopmentDatabaseSetup(context);
            testDataGenerator.SetupDevelopmentDataSet();
        }

        [Test]
        public void TestSuccessfulLogin()
        {
            UserController userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());  
            ActionResult<LoggedInDto> response = userController.Login(new LoginDto
            {
                Username = "user",
                Password = "password"
            });

            Assert.IsNotNull(response.Value);
            Assert.IsFalse(string.IsNullOrEmpty(response.Value.Username));
            Assert.IsFalse(string.IsNullOrEmpty(response.Value.Token));
        }

        [Test]
        public void TestIncorrectUsernameLogin()
        {
            UserController userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
            ActionResult<LoggedInDto> response = userController.Login(new LoginDto
            {
                Username = "UserDoesNotExist",
                Password = "password"
            });

            Assert.IsNull(response.Value);
        }

        [Test]
        public void TestIncorrectPasswordLogin()
        {
            UserController userController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
            ActionResult<LoggedInDto> response = userController.Login(new LoginDto
            {
                Username = "user",
                Password = "IncorrectPassword"
            });

            Assert.IsNull(response.Value);
        }

        [Test]
        public void TestRegistration()
        {
            UserController initialLoginUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
            ActionResult<LoggedInDto> initialLoginResponse = initialLoginUserController.Login(new LoginDto
            {
                Username = "newUser",
                Password = "newUserPassword"
            });

            Assert.IsNull(initialLoginResponse.Value);

            UserController registrationUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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

            UserController finalLoginUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
            ActionResult<LoggedInDto> finalLoginResponse = finalLoginUserController.Login(new LoginDto
            {
                Username = "newUser",
                Password = "newUserPassword"
            });

            Assert.IsNotNull(finalLoginResponse.Value);
            Assert.IsFalse(string.IsNullOrEmpty(finalLoginResponse.Value.Username));
            Assert.IsFalse(string.IsNullOrEmpty(finalLoginResponse.Value.Token));

            Assert.AreEqual(finalLoginResponse.Value.Username, registrationResponse.Value.UserName);
        }

        [Test]
        public void TestRegistrationWithDuplicateUserName()
        {
            UserController registrationOneUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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

            UserController registrationTwoUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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

        [Test]
        public void TestRegistrationWithDuplicateEmail ()
        {
            UserController registrationOneUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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

            UserController registrationTwoUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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

        [Test]
        public void TestRegistrationWithDuplicatePhoneNumber()
        {
            UserController registrationOneUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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

            UserController registrationTwoUserController = new UserController(_serviceProvider.GetService<IOptions<AppSettings>>());
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
    }
}
