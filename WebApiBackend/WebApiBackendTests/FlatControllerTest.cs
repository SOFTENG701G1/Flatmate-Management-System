using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.Model;
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
        /// Ensures that a user already in the flat can't be added again
        /// </summary>
        [Test]
        public void TestAddExistingUserInFlat()
        {
            // Arrange
            var username = "YinWang";

            // Act
            var response = _flatController.AddUserToFlat(username);
            
            // Assert
            Assert.AreEqual(response.Value.ResultCode, 4);
        }

        /// <summary>
        /// Ensures that a non-existent user can't be added to a flat
        /// </summary>
        [Test]
        public void TestAddNonExistentUserToFlat()
        {
            // Arrange
            var username = "Bazinga";

            // Act
            var response = _flatController.AddUserToFlat(username);

            // Assert
            Assert.AreEqual(response.Value.ResultCode, 2);
        }


        [Test]
        public void TestFailedAddUserToFlatUserInOtherFlat()
        {
            ActionResult<AddUserToFlatDto> response = _flatController.AddUserToFlat("TestUser1");
            Assert.AreEqual(response.Value.ResultCode, 5);
        }

        /// <summary>
        /// Ensures that a user can be added to a flat
        /// </summary>
        [Test]
        public void TestAddUserToFlat()
        {
            // Arrange
            var username = "TestUser2";

            // Act
            var response = _flatController.AddUserToFlat(username);

            // Assert
            Assert.AreEqual(response.Value.ResultCode, 1);
            Assert.AreEqual(response.Value.AddedUser.UserName, username);
        }

        /// <summary>
        /// Test that in a normal flat, all members are returned, including the person themself
        /// </summary>
        [Test]
        public void GetFlatMembersFromNonEmptyFlat()
        {
            // Arrange
            
            // Act
            var response = _flatController.GetFlatMembers();

            // Assert
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 4, true);
        }

        [Test]
        public void GetFlatMembersFromSoloFlat()
        {
            // Arrange
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "998") });
            _flatController.HttpContext.User = new ClaimsPrincipal(objClaim);

            // Act
            var response = _flatController.GetFlatMembers();

            // Assert
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 1, true);
        }

        [Test]
        public void GetFlatMembersFromPersonNotInFlat()
        {
            // Arrange
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "999") });
            _flatController.HttpContext.User = new ClaimsPrincipal(objClaim);

            // Act
            var response = _flatController.GetFlatMembers();

            // Assert
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 0, true);
        }

        [Test]
        public void AttemptCreatingFlatWhileAlreadyInFlat()
        {
            // Arrange

            // Act
            var response = _flatController.createFlat();

            // Assert
            Assert.That(response.Result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public void TestCreatingFlatWhileNotInFlat()
        {
            // Arrange
            ClaimsIdentity objClaim = new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "999") });
            _flatController.HttpContext.User = new ClaimsPrincipal(objClaim);

            // Act
            var response = _flatController.createFlat();

            // Assert
            Assert.IsInstanceOf<FlatDTO>(response.Value);
            Assert.AreEqual(response.Value.FlatMembers.Count == 1, true);
        }

    }
}