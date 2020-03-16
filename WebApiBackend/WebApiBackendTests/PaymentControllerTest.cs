using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{
    class PaymentControllerTest
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

        [Test]
        public void TestAddUserToExistingPayment()
        {
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
        public void TestGetPaymentsForFlat()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGetAllPaymentsForUser()
        {
            Assert.Pass();
        }
    }
}
