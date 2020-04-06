using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApiBackend.Dto;
using WebApiBackend.Model;
using WebApiBackendTests.Helper;
using System;

namespace WebApiBackendTests
{
    class ChoreMapperTest
    {
        private MapperHelper _mapperHelper;

        [SetUp]
        public void Setup()
        {
            _mapperHelper = new MapperHelper();
        }

        [Test]
        public void TestChoreMapping()
        {
            //create test user
            User testUser = new User
            {
                Id = 22,
                UserName = "JohnSmith123",
                FirstName = "John",
                LastName = "Smith",
            };
            //example chore
            Chores myChore = new Chores
            {
                Id = 1,
                Title = "dishes",
                Description = "do the dishes",
                AssignedUser = testUser,
                DueDate = new DateTime(2020, 04, 04),
                Completed = false,
                Recurring = true,
            };

            //Object->DTO Test
            ChoresDTO myDTO = new ChoresDTO(myChore);
            Assert.AreEqual(myDTO.Id, myChore.Id);
            Assert.AreEqual(myDTO.Title, myChore.Title);
            Assert.AreEqual(myDTO.Description, myChore.Description);
            Assert.AreEqual(myDTO.Assignee, myChore.AssignedUser.Id);
            Assert.AreEqual(myDTO.DueDate, myChore.DueDate);
            Assert.AreEqual(myDTO.Completed, myChore.Completed);
            Assert.AreEqual(myDTO.Recurring, myChore.Recurring);

            var mapper = _mapperHelper.GetMapper();

            //DTO->Object Test
            Chores backToChore = mapper.Map<ChoresDTO, Chores>(myDTO);
            //in the controller, you would write here "User user = await _userRepository.Get(userId);"
            //but since we don't have that here, we'll assume it we have the user object
            backToChore.AssignedUser = testUser;

            Assert.AreEqual(myDTO.Id, backToChore.Id);
            Assert.AreEqual(myDTO.Title, backToChore.Title);
            Assert.AreEqual(myDTO.Description, backToChore.Description);
            Assert.AreEqual(myDTO.Assignee, backToChore.AssignedUser.Id);
            Assert.AreEqual(myDTO.DueDate, backToChore.DueDate);
            Assert.AreEqual(myDTO.Completed, backToChore.Completed);
            Assert.AreEqual(myDTO.Recurring, backToChore.Recurring);
        }
    }
}
