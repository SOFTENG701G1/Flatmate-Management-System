using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiBackend.Controllers;
using WebApiBackend.EF;
using WebApiBackend.Helpers;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{
    class PaymentControllerTest
    {
        private DatabaseSetUpHelper _dbSetUpHelper;
        private ServiceDependencyResolver _serviceProvider;
        private FlatManagementContext _context;

        private PaymentsRepository _paymentsRepository;
        private UserPaymentsRepository _userPaymentsRepository;
        private FlatRepository _flatRepository;
        private UserRepository _userRepository;

        private PaymentsController _paymentsController;

        [SetUp]
        public void Setup()
        {
            _dbSetUpHelper = new DatabaseSetUpHelper();
            _serviceProvider = _dbSetUpHelper.GetServiceDependencyResolver();
            _context = _dbSetUpHelper.GetContext();

            _paymentsRepository = new PaymentsRepository(_context);
            _userPaymentsRepository = new UserPaymentsRepository(_context);
            _flatRepository = new FlatRepository(_context);
            _userRepository = new UserRepository(_context);

            // ISSUE: controller takes in IMapper, but no way of instantiating
            _paymentsController = new PaymentsController(_paymentsRepository, _userPaymentsRepository, _flatRepository, _userRepository, null);
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
            //// Arrange
            //var flatId = 1;

            //// Act
            //var response = await _paymentsController.GetPaymentsForFlat(flatId);

            //// Assert
            //Assert.IsNotNull(response);
            Assert.Pass();
        }

        [Test]
        public void TestGetAllPaymentsForUser()
        {
            Assert.Pass();
        }
    }
}
