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
            Assert.That(response.Value.Count, Is.EqualTo(3));
            Assert.That(response.Value.Select(m => m.UserName).ToList(), Is.EquivalentTo(new[] { "BeboBryan", "TreesAreGreen", "YinWang" }));
            Assert.That(response.Value.Select(m => m.FirstName).ToList(), Is.EquivalentTo(new[] { "Bryan", "Teresa", "Yin" }));
            Assert.That(response.Value.Select(m => m.LastName).ToList(), Is.EquivalentTo(new[] { "Ang", "Green", "Wang" }));
            Assert.That(response.Value.Select(m => m.Email).ToList(), Is.EquivalentTo(new[] { "BryanAng@Gmail.com", "GreenTrees@Yahoo.com", "YinWang@qq.com" }));


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
            Flat flat = _context.Flat.First(u=>u.Id==user.FlatId);
            int flatId = flat.Id;
            ActionResult response = _flatController.RemoveMember("YinWang");
            Assert.IsFalse(flat.Users.Contains(user));
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