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

        /// <summary>
        /// Ensures that chores can be retrieved for a flat
        /// </summary>
        [Test]
        public async Task TestGetChoresForFlatAsync()
        {
            var chore = new Chore
            {
                Title = "dishes",
                Description = "do the dishes",
                AssignedUser = new User(),
                DueDate = new DateTime(2020, 04, 04),
                Completed = false,
                Recurring = true,
            };


            await _choresRepository.Add(chore);

            // Act
            var response = await _choreController.GetAllChoresForFlat();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        /// <summary>
        /// Ensures that a chore can be deleted by the correct user
        /// </summary>
        [Test]
        public async Task TestDeleteChoreCorrectUserAsync()
        {
            // Arrange
            var chore = new Chore
            {
                Title = "dishes",
                Description = "do the dishes",
                AssignedUser = await _userRepository.Get(1),    // Chore has to be assigned to a real user
                DueDate = new DateTime(2020, 04, 04),
                Completed = false,
                Recurring = true,
            };

            Chore result = await _choresRepository.Add(chore);
            int id = result.Id;

            // We need to ensure that the chore is added to the flat belonging to the active user
            // (The active user and flat has been initialised by WebApiBackendTests.Helper.DatabaseSetUpHelper
            //    using WebApiBackend.Helpers.DevelopmentDatabaseSetup)
            ClaimsIdentity identity = _httpContextHelper.GetClaimsIdentity();
            int userId = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            User activeUser = await _userRepository.Get(userId);
            activeUser.Flat.Chores.Add(result);

            // Act
            var response = await _choreController.DeleteChore(id);

            // Assert
            Assert.IsInstanceOf<OkResult>(response);
            Assert.Null(await _choresRepository.Get(id));
        }

        /// <summary>
        /// Ensures that chores can be retrieved for a flat
        /// </summary>
        [Test]
        public async Task TestMarkChoreAsCompletedAsync()
        {
            var chore = new Chore
            {
                Title = "dishes",
                Description = "do the dishes",
                AssignedUser = new User(),
                DueDate = new DateTime(2020, 04, 04),
                Completed = false,
                Recurring = true,
            };


            await _choresRepository.Add(chore);

            Chore r_chore = await _choresRepository.Get(chore.Id);

            // Assert
            Assert.AreEqual(r_chore.Completed, false);
            var response = await _choreController.MarkChoreAsCompleted(chore.Id);
            r_chore = await _choresRepository.Get(chore.Id);
            Assert.AreEqual(r_chore.Completed, true);
            Assert.AreEqual(r_chore.Id, chore.Id);

            response = await _choreController.MarkChoreAsCompleted(chore.Id);
            r_chore = await _choresRepository.Get(chore.Id);
            Assert.AreEqual(r_chore.Completed, false);

        }
    }
}
