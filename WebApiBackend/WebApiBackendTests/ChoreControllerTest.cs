using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Diagnostics;
using WebApiBackend.Controllers;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        /// Checks for the equality of Chores. This is here because the functionality for checking value equality
        /// of Chores is unavailable. Because of the rules for Pull Requests, it cannot be added in the same PR as
        /// the tests and must be added to the Chore class later, if required elsewhere.
        /// </summary>
        private bool ChoreEquals(Chore choreOne, Chore choreTwo)
        {
            return (choreOne.Id.Equals(choreTwo.Id) &&
               choreOne.Title.Equals(choreTwo.Title) &&
               choreOne.Description.Equals(choreTwo.Description) &&
               choreOne.AssignedUser.Id.Equals(choreTwo.AssignedUser.Id) &&
               choreOne.DueDate.Equals(choreTwo.DueDate) &&
               choreOne.Completed.Equals(choreTwo.Completed) &&
               choreOne.Recurring.Equals(choreTwo.Recurring)
               );
        }

        /// <summary>
        /// Ensures that a chore can be created for a flat
        /// </summary>
        [Test]
        public async Task TestCreateChoreForFlatAsync()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            
            // Arrange
            var chore = new ChoreDTO
            {
                Title = "lawn",
                Description = "mow the lawn",
                Assignee = 1,
                DueDate = new DateTime(2020, 05, 05),
                Completed = false,
                Recurring = true,
            };

            int userId = 1; // as initialised in HttpContextHelper
            List<Chore> originalChores = await _choresRepository.GetAll();

            // Act
            var response = await _choreController.CreateChoreForFlat(chore);

            // Assert OK result and Chore has been added to repository
            Assert.IsInstanceOf<OkResult>(response);

            Chore modelChore = new Chore
            {
                Id = 3, // this is what we expect based on DevelopmentDatabaseSetup intialisation
                Title = "lawn",
                Description = "mow the lawn",
                AssignedUser = await _userRepository.Get(1),
                DueDate = new DateTime(2020, 05, 05),
                Completed = false,
                Recurring = true,
            };

            // The only difference between the chore repo before and after calling the should
            List<Chore> newChores = await _choresRepository.GetAll();
            newChores.RemoveAll(c => originalChores.Contains(c));
            Assert.AreEqual(1, newChores.Count);
            Assert.True(ChoreEquals(newChores[0], modelChore));
        }

        /// <summary>
        /// Ensure that a BadRequest is returned when a request is made for
        /// to create a chore with an asignee who doesn't exist
        /// </summary>
        [Test]
        public async Task TestCreateChoreNonexistentAssignee()
        {
            // Arrange
            var chore = new ChoreDTO
            {
                Title = "dishes",
                Description = "do the dishes",
                Assignee = 100,
                DueDate = new DateTime(2020, 04, 04),
                Completed = false,
                Recurring = true,
            };

            // Act
            var response = await _choreController.CreateChoreForFlat(chore);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        /// <summary>
        /// Ensures that chores can be retrieved for a flat
        /// </summary>
        [Test]
        public async Task TestGetChoresForFlatAsync()
        {
            var chore = new Chore
            {
                Title = "lawn",
                Description = "mow the lawn",
                AssignedUser = await _userRepository.Get(1),
                DueDate = new DateTime(2021, 05, 05),
                Completed = false,
                Recurring = true,
            };

            Chore result = await _choresRepository.Add(chore);

            // We need add the chore to the flat belonging to the active user
            int userId = 1; // as initialised in HttpContextHelper
            User activeUser = await _userRepository.Get(userId);
            Flat userFlat = activeUser.Flat;
            userFlat.Chores.Add(result);
            await _flatRepository.Update(userFlat);

            // Act
            var response = await _choreController.GetAllChoresForFlat();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(response);
            OkObjectResult actionResult = (OkObjectResult)response;
            Assert.IsInstanceOf<List<ChoreDTO>>(actionResult.Value);
            List<ChoreDTO> actualChores = (List<ChoreDTO>)actionResult.Value;

            ICollection<ChoreDTO> expectedChores = new List<ChoreDTO> {
                new ChoreDTO 
                {
                    Id = 1,
                    Title = "dishes",
                    Description = "do the dishes",
                    Assignee = 1,
                    DueDate = new DateTime(2020, 04, 04),
                    Completed = false,
                    Recurring = true
                }, new ChoreDTO 
                {
                    Id = 2,
                    Title = "rubbish",
                    Description = "take out the rubbish",
                    Assignee = 3,
                    DueDate = new DateTime(2020, 05, 05),
                    Completed = false,
                    Recurring = true
                }, new ChoreDTO
                {
                    Id = 3,
                    Title = "lawn",
                    Description = "mow the lawn",
                    Assignee = 1,
                    DueDate = new DateTime(2021, 05, 05),
                    Completed = false,
                    Recurring = true,
                }
            };

            //Assert.AreEqual(expectedChores, actualChores);
            Assert.AreEqual(expectedChores.Count, actualChores.Count);
            bool allPresent = true;
            foreach (ChoreDTO c in expectedChores)
            {
                bool found = false;
                foreach (ChoreDTO cPrime in actualChores)
                {
                    if (c.Id.Equals(cPrime.Id) &&
                        c.Title.Equals(cPrime.Title) &&
                        c.Description.Equals(cPrime.Description) &&
                        c.Assignee.Equals(cPrime.Assignee) &&
                        c.DueDate.Equals(cPrime.DueDate) &&
                        c.Completed.Equals(cPrime.Completed) &&
                        c.Recurring.Equals(cPrime.Recurring))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    allPresent = false;
                    break;
                }
            }

            Assert.True(allPresent, "Not all chores in the expected results returned by GetAllChoresForFlat " +
                "were present in the actual results");
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

            int userId = 1; // as initialised in HttpContextHelper
            User activeUser = await _userRepository.Get(userId);
            Flat userFlat = activeUser.Flat;
            userFlat.Chores.Add(result);
            await _flatRepository.Update(userFlat);

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
