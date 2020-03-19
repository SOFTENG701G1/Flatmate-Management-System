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


         /// <summary>
        /// Ensure the user is excluded from the flat and return the new flat member list after removal
        /// </summary>
        [Test]
        public void GetMember_Return202_WhenUserNotRemovingOneself()
        {
            User user = _context.User.Find(1);
            User deleteUser = _context.User.Where(u => u.UserName == "BeboBryan").FirstOrDefault();
            IQueryable<Flat> flat = _context.Flat.Where(f => f.Id == user.FlatId);
            ActionResult<FlatDTO> response = _flatController.RemoveMember("BeboBryan");
            Assert.IsFalse(flat.FirstOrDefault().Users.Contains(deleteUser));
            Assert.That(response.Value, Is.TypeOf<FlatDTO>());
        }
        
        
        /// <summary>
        /// Ensure a user is excluded from the flat's user list
        /// </summary>
        [Test]
        public void GetMember_RirectToCreateFlatPage_WhenUserRemoveOneselfFromFlat()
        {
            User user = _context.User.Find(1);
            Flat flat = _context.Flat.First(u=>u.Id==user.FlatId);
            int flatId = flat.Id;
            ActionResult<FlatDTO> response = _flatController.RemoveMember("YinWang");
            Assert.IsFalse(flat.Users.Contains(user));

        }


        /// <summary>
        /// Ensure the Flat is removed when the last user left the flat
        /// </summary>
        [Test]
        public void GetMember_RirectToCreateFlatPageandRemoveFlat_WhenTheLastUserRemovedFromFlat()
        {
            IQueryable<Flat> flat = _context.Flat.Where(f => f.Id == _context.User.Find(1).FlatId);
            ActionResult<FlatDTO> response = _flatController.RemoveMember("YinWang");
            Assert.IsEmpty(flat);

        }
    }
}