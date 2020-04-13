using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;

namespace WebApiBackendTests
{

    class ChoreControllerTest
    {
        private DatabaseSetUpHelper _dbSetUpHelper;
        private FlatManagementContext _context;
        private MapperHelper _mapperHelper;
        private HttpContextHelper _httpContextHelper;

        private ChoresRepository _choresRepository;
        private FlatRepository _flatRepository;
        private UserRepository _userRepository;

        private ChoreController _choreController;

        [SetUp]
        public void Setup()
        {
            _dbSetUpHelper = new DatabaseSetUpHelper();
            _context = _dbSetUpHelper.GetContext();
            _httpContextHelper = new HttpContextHelper();

            _choresRepository = new ChoresRepository(_context);
            _flatRepository = new FlatRepository(_context);
            _userRepository = new UserRepository(_context);

            _mapperHelper = new MapperHelper();
            var mapper = _mapperHelper.GetMapper();

            var httpContext = _httpContextHelper.GetHttpContext();
            var objClaim = _httpContextHelper.GetClaimsIdentity();

            _choreController = new ChoreController(_choresRepository, _flatRepository, _userRepository, mapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            httpContext.User = new ClaimsPrincipal(objClaim);
        }

        /// <summary>
        /// Ensures that a payment can be created for a flat
        /// </summary>
        [Test]
        public async Task TestCreateChoreForFlatAsync()
        {
            // Arrange
            var chore = new ChoreDTO
            {
                Title = "dishes",
                Description = "do the dishes",
                Assignee = 1,
                DueDate = new DateTime(2020, 04, 04),
                Completed = false,
                Recurring = true,
            };

            // Act
            var response = await _choreController.CreateChoreForFlat(chore);

            // Assert
            Assert.IsInstanceOf<OkResult>(response);
        }
    }
}
