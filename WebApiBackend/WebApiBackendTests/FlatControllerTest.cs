using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Linq;
using WebApiBackend.Controllers;
using WebApiBackend.Model;
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
        /// Ensure a user is able to view a list member in the flat. Ensure the response contains the expected members
        /// </summary>
        [Test]
        public void TestGetMemberList()
        {
            // Arrange
            var usernames = new[] { "BeboBryan", "TreesAreGreen", "YinWang", "TonOfClay" };
            var firstNames = new[] { "Bryan", "Teresa", "Yin", "Clay" };
            var lastNames = new[] { "Ang", "Green", "Wang", "Ton" };
            var emails = new[] { "BryanAng@Gmail.com", "GreenTrees@Yahoo.com", "YinWang@qq.com", "ClayTon@Gmail.com" };

            // Act
            var response = _flatController.GetMembers();

            // Assert
            Assert.IsNotNull(response.Value);
            Assert.That(response.Value.Count, Is.EqualTo(4));
            Assert.That(response.Value.Select(m => m.UserName).ToList(), Is.EquivalentTo(usernames));
            Assert.That(response.Value.Select(m => m.FirstName).ToList(), Is.EquivalentTo(firstNames));
            Assert.That(response.Value.Select(m => m.LastName).ToList(), Is.EquivalentTo(lastNames));
            Assert.That(response.Value.Select(m => m.Email).ToList(), Is.EquivalentTo(emails));
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

        /// <summary>
        /// Ensures that a user that already has a flat can't be added to another flat
        /// </summary>
        [Test]
        public void TestAddUserWithFlatToAnotherFlat()
        {
            // Arrange
            var username = "TestUser1";

            // Act
            var response = _flatController.AddUserToFlat(username);

            // Assert
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
    }
}
