using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{
    class PaymentControllerTest
    {
        private DatabaseSetUpHelper _dbSetUpHelper;
        private FlatManagementContext _context;
        private MapperHelper _mapperHelper;
        private HttpContextHelper _httpContextHelper;

        private PaymentsRepository _paymentsRepository;
        private UserPaymentsRepository _userPaymentsRepository;
        private FlatRepository _flatRepository;
        private UserRepository _userRepository;

        private PaymentsController _paymentsController;

        [SetUp]
        public void Setup()
        {
            _dbSetUpHelper = new DatabaseSetUpHelper();
            _context = _dbSetUpHelper.GetContext();
            _httpContextHelper = new HttpContextHelper();

            _paymentsRepository = new PaymentsRepository(_context);
            _userPaymentsRepository = new UserPaymentsRepository(_context);
            _flatRepository = new FlatRepository(_context);
            _userRepository = new UserRepository(_context);

            _mapperHelper = new MapperHelper();
            var mapper = _mapperHelper.GetMapper();
            
            var httpContext = _httpContextHelper.GetHttpContext();
            var objClaim = _httpContextHelper.GetClaimsIdentity();

            _paymentsController = new PaymentsController(_paymentsRepository, _userPaymentsRepository, _flatRepository, _userRepository, mapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            httpContext.User = new ClaimsPrincipal(objClaim);
        }

        [Test]
        public async Task TestAddUserToExistingPaymentAsync()
        {
            // Arrange
            var paymentId = 1;
            var userId = 4;

            // Act
            var response = await _paymentsController.AddUserToExistingPayment(paymentId, userId);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task TestCreatePaymentForFlatAsync()
        {
            // Arrange
            var payment = new PaymentDTO
            {
                Amount = 99,
                PaymentType = PaymentType.Other,
                Frequency = Frequency.Weekly,
                StartDate = new DateTime(2020, 04, 04),
                EndDate = new DateTime(2020, 05, 05),
                Fixed = false,
                Description = "food",
            };
            var userIds = new List<int> { 1, 2 };

            // Act
            var response = await _paymentsController.CreatePaymentForFlat(payment, userIds);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public async Task TestDeletePaymentForFlatAsync()
        {
            // Arrange
            var paymentId = 2;

            // Act
            var response = await _paymentsController.DeletePaymentForFlat(paymentId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public async Task TestDeleteUserFromPaymentAsync()
        {
            // Arrange
            var paymentId = 1;
            var userId = 1;

            // Act
            var response = await _paymentsController.DeleteUserFromPayment(paymentId, userId);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task TestEditPaymentAsync()
        {
            // Arrange
            var payment = new PaymentDTO
            {
                Amount = 120,
                PaymentType = PaymentType.Electricity,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 04, 04),
                EndDate = new DateTime(2020, 05, 05),
                Fixed = false,
                Description = "electricity",
            };

            // Act
            var response = await _paymentsController.Put(1, payment);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task TestGetPaymentsForFlatAsync()
        {
            // Arrange

            // Act
            var response = await _paymentsController.GetPaymentsForFlat();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public async Task TestGetAllPaymentsForUserAsync()
        {
            // Arrange

            // Act
            var response = await _paymentsController.GetAllPaymentsForUser();

            // Assert
            Assert.IsNotNull(response);
        }
    }
}
