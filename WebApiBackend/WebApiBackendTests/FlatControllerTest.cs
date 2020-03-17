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

        /// <summary>
        /// Ensure the response is a ok reuslt when the user is not removing him/herself, and the user is excluded from the flat's user list 
        /// </summary>
        [Test]
        public void GetMember_ReturnOK_WhenUserNotRemovingOneself()
        {
            User user = _context.User.Find(1);
            User deleteUser = _context.User.Where(u => u.UserName == "BeboBryan").FirstOrDefault();
            IQueryable<Flat> flat = _context.Flat.Where(f => f.Id == user.FlatId);
            ActionResult response = _flatController.RemoveMember("BeboBryan");
            Assert.IsFalse(flat.FirstOrDefault().Users.Contains(deleteUser));
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
        }

        /// <summary>
        /// Ensure the response is a RedirectToActionResult when the user is removing him/herself, and the user is excluded from the flat's user list
        /// </summary>

        [Test]
        public void GetMember_RirectToCreateFlatPage_WhenUserRemoveOneselfFromFlat()
        {
            User user = _context.User.Find(1);
            IQueryable<Flat> flat = _context.Flat.Where(f => f.Id == user.FlatId);
            int flatId = flat.FirstOrDefault().Id;
            ActionResult response = _flatController.RemoveMember("YinWang");
            //Assert.IsFalse(flat.Users.Contains(user));
            Assert.That(response, Is.InstanceOf<RedirectToActionResult>());
        }

        /// <summary>
        /// Ensure the Flat is removed when the last user left the flat, and be redirect to action
        /// </summary>
        [Test]
        public void GetMember_RirectToCreateFlatPageandRemoveFlat_WhenTheLastUserRemovedFromFlat()
        {
            IQueryable<Flat> flat = _context.Flat.Where(f => f.Id == _context.User.Find(1).FlatId);
            ActionResult response = _flatController.RemoveMember("YinWang");
            Assert.That(response, Is.InstanceOf<RedirectToActionResult>());
            Assert.IsEmpty(flat);

        }
    }
}
