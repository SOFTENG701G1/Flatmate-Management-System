using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Helpers;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{
    class PaymentControllerTest
    {
        private DatabaseSetUpHelper _dbSetUpHelper;
        private FlatManagementContext _context;

        private MapperHelper _mapperHelper;

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

            _paymentsRepository = new PaymentsRepository(_context);
            _userPaymentsRepository = new UserPaymentsRepository(_context);
            _flatRepository = new FlatRepository(_context);
            _userRepository = new UserRepository(_context);

            _mapperHelper = new MapperHelper();
            var mockMapper = _mapperHelper.GetMapper();

            _paymentsController = new PaymentsController(_paymentsRepository, _userPaymentsRepository, _flatRepository, _userRepository, mockMapper);
        }

        [Test]
        public void TestAddUserToExistingPayment()
        {
            // Act
            
            // Assert
            Assert.Pass();
        }

        [Test]
        public void TestCreatePaymentForFlat()
        {
            Assert.Pass();
        }

        [Test]
        public void TestDeletePaymentForFlat()
        {
            Assert.Pass();
        }

        [Test]
        public void TestDeleteUserFromPayment()
        {
            Assert.Pass();
        }

        [Test]
        public void TestEditPayment()
        {
            Assert.Pass();
        }

        [Test]
        public async Task TestGetPaymentsForFlatAsync()
        {
            // Arrange
            var flatId = 1;

            // Act
            var response = await _paymentsController.GetPaymentsForFlat(flatId);

            // Assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void TestGetAllPaymentsForUser()
        {
            Assert.Pass();
        }
    }
}
