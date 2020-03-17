using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WebApiBackend.Model;
using WebApiBackend.Helpers;
using WebApiBackend;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBackendTests
{
    
    class FlatControllerTest
    {
        private FlatManagementContext _context;
        ServiceDependencyResolver _serviceProvider;

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
        [Authorize]
        public void TestFailedAddUserToFlatUserAlreadyInFlat()
        {
            FlatController ctl = new FlatController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);

            ActionResult<AddUserToFlatDto> response = ctl.AddUserToFlat(1,1);
            Assert.AreEqual(response.Value.ResultCode,4);
        }

        [Test]
        [Authorize]
        public void TestFailedAddUserToFlatUserNotExist()
        {
            FlatController ctl = new FlatController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);

            ActionResult<AddUserToFlatDto> response = ctl.AddUserToFlat(1, 100);
            Assert.AreEqual(response.Value.ResultCode, 2);
        }

        [Test]
        [Authorize]
        public void TestFailedAddUserToFlatFlatNotExist()
        {
            FlatController ctl = new FlatController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);

            ActionResult<AddUserToFlatDto> response = ctl.AddUserToFlat(10, 1);
            Assert.AreEqual(response.Value.ResultCode, 3);
        }

        [Test]
        [Authorize]
        public void TestFailedAddUserToFlatUserInOtherFlat()
        {
            FlatController ctl = new FlatController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);

            ActionResult<AddUserToFlatDto> response = ctl.AddUserToFlat(1, 3);
            Assert.AreEqual(response.Value.ResultCode, 5);
        }

        [Test]
        [Authorize]
        public void TestCorrectAddUserToFlat()
        {
            FlatController ctl = new FlatController(_serviceProvider.GetService<IOptions<AppSettings>>(), _context);

            ActionResult<AddUserToFlatDto> response = ctl.AddUserToFlat(1, 2);
            Assert.AreEqual(response.Value.ResultCode, 1);
        }
    }
}



