using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using WebApiBackend;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.Helpers;
using WebApiBackend.Model;
using System.Security.Claims;

namespace WebApiBackendTests
{
    class FlatTest
    {
        ServiceDependencyResolver _serviceProvider;
        private FlatManagementContext _context;
        private FlatController _flatController;

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

            _flatController = new FlatController(_context);

            //Creates a new httpContext and adds a user identity to it, imitating being already logged in.
            DefaultHttpContext httpContext = new DefaultHttpContext();
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") });
            httpContext.User = new ClaimsPrincipal(objClaim);

            _flatController.ControllerContext = new ControllerContext();
            _flatController.ControllerContext.HttpContext = httpContext;
            


        }


        [Test]
        public void TestFailedAddUserToFlatUserAlreadyInFlat()
        {
            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("YinWang");
            Assert.AreEqual(response.Value.ResultCode, 4);
        }

        [Test]
        public void TestFailedAddUserToFlatUserNotExist()
        {
            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("Bazinga");
            Assert.AreEqual(response.Value.ResultCode, 2);
        }


        [Test]
        public void TestFailedAddUserToFlatUserInOtherFlat()
        {
            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("TestUser1");
            Assert.AreEqual(response.Value.ResultCode, 5);
        }

        [Test]
        public void TestCorrectAddUserToFlat()
        {
            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("TestUser2");
            Assert.AreEqual(response.Value.ResultCode, 1);
            Assert.AreEqual(response.Value.AddedUser.UserName, "TestUser2");
        }

        //Test that in a normal flat, all members are returned, including the person themself
        [Test]
        public void GetFlatMembersFromNonEmptyFlat()
        {
            ActionResult<FlatDTO> response = _flatController.GetFlatMembers();
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 4, true);
        }

        [Test]
        public void GetFlatMembersFromSoloFlat()
        {
            //Set the user of the context to be user with id 998, who is in a flat of their own.
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "998") });
            _flatController.HttpContext.User = new ClaimsPrincipal(objClaim);

            ActionResult<FlatDTO> response = _flatController.GetFlatMembers();
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 1, true);
        }

        [Test]
        public void GetFlatMembersFromPersonNotInFlat()
        {
            //Set the user of the context to be user with id 999, who is not in a flat.
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "999") });
            _flatController.HttpContext.User = new ClaimsPrincipal(objClaim);

            //Should still return a flat DTO, but contain no inputs in the FlatMembers Array.
            ActionResult<FlatDTO> response = _flatController.GetFlatMembers();
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 0, true);
        }

        [Test]
        public void AttemptCreatingFlatWhileAlreadyInFlat()
        {

            ActionResult<FlatDTO> response = _flatController.createFlat();
            Assert.That(response.Result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public void TestCreatingFlatWhileNotInFlat()
        {
            //Set the user of the context to be user with id 999, who is not in a flat.
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "999") });
            _flatController.HttpContext.User = new ClaimsPrincipal(objClaim);

            //Should return a flat DTO, Cotaining just the user.
            ActionResult<FlatDTO> response = _flatController.createFlat();
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 1, true);
        }

    }
}
