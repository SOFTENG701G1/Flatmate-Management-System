using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBackend;
using WebApiBackend.Helpers;
using WebApiBackend.Model;

namespace WebApiBackendTests
{
    class PaymentControllerTest
    {
        ServiceDependencyResolver _serviceProvider;
        private FlatManagementContext _context;

        [SetUp]
        public void Setup()
        {
            // Builds webhost and gets service providers from web host
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _serviceProvider = new ServiceDependencyResolver(webHost);

            // Resets database to inital state so all tests are isolated and repeatable
            _context = new FlatManagementContext();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var testDataGenerator = new DevelopmentDatabaseSetup(_context);
            testDataGenerator.SetupDevelopmentDataSet();
        }

        [Test]
        public void TestCreatePayment()
        {
            Assert.Pass();
        }

        [Test]
        public void TestDeletePayment()
        {
            Assert.Pass();
        }

        [Test]
        public void TestEditPayment()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGetFlatPayments()
        {
            Assert.Pass();
        }

        [Test]
        public void TestGetUserPayments()
        {
            Assert.Pass();
        }
    }
}
