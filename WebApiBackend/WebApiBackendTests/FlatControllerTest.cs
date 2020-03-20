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
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{
    class FlatTest
    {
        private DatabaseSetUpHelper _dbSetUpHelper;
        private FlatManagementContext _context;
        private HttpContextHelper _httpContextHelper;

        private FlatController _flatController;

        [SetUp]
        public void Setup()
        {
            _dbSetUpHelper = new DatabaseSetUpHelper();
            _context = _dbSetUpHelper.GetContext();
            _httpContextHelper = new HttpContextHelper();

            var httpContext = _httpContextHelper.GetHttpContext();
            var objClaim = _httpContextHelper.GetClaimsIdentity();

            _flatController = new FlatController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
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
            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("TestUser2");
            Assert.AreEqual(response.Value.ResultCode, 1);
            Assert.AreEqual(response.Value.AddedUser.UserName, "TestUser2");
        }

    }
}
