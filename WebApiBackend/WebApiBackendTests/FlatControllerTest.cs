using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiBackend;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.Helpers;
using WebApiBackend.Model;
using Moq;
using System.Security.Principal;
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
            GenericIdentity MyIdentity = new GenericIdentity("YinWang");
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "1") });
            _flatController.ControllerContext = new ControllerContext();
            _flatController.ControllerContext.HttpContext = httpContext;
            httpContext.User = new ClaimsPrincipal(objClaim);


        }

        /// <summary>
        /// Ensure a user is able to view a list memeber in the flat. Ensure the reponse contains the expected members
        /// </summary>
        [Test]
        public void TestGetMemberList()
        {
            ActionResult<List<DisplayMemberDTO>> response = _flatController.GetMembers();

            Assert.IsNotNull(response.Value);
            Assert.That(response.Value.Count, Is.EqualTo(4));
            Assert.That(response.Value.Select(m => m.UserName).ToList(), Is.EquivalentTo(new[] { "BeboBryan", "TreesAreGreen", "YinWang", "TonOfClay" }));
            Assert.That(response.Value.Select(m => m.FirstName).ToList(), Is.EquivalentTo(new[] { "Bryan", "Teresa", "Yin", "Clay" }));
            Assert.That(response.Value.Select(m => m.LastName).ToList(), Is.EquivalentTo(new[] { "Ang", "Green", "Wang", "Ton" }));
            Assert.That(response.Value.Select(m => m.Email).ToList(), Is.EquivalentTo(new[] { "BryanAng@Gmail.com", "GreenTrees@Yahoo.com", "YinWang@qq.com", "ClayTon@Gmail.com" }));
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
            //FlatController ctl = new FlatController( _context);

            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("TestUser2");
            Assert.AreEqual(response.Value.ResultCode, 1);
        }

    }
}
