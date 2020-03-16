using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

namespace WebApiBackendTests
{
    class FlatTest
    {
        ServiceDependencyResolver _serviceProvider;
        private FlatManagementContext _context;

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
        }

        /// <summary>
        /// Ensure a user is able to view a list memeber in the flat. Ensure the reponse contains the expected members
        /// </summary>
        [Test]
        public void TestGetMemberList()
        {
            FlatController flatController = new FlatController(_context);
            ActionResult<List<DisplayMemberDTO>> response = flatController.GetMembers(1);

            Assert.IsNotNull(response.Value);
            Assert.That(response.Value.Count, Is.EqualTo(3));
            Assert.That(response.Value.Select(m => m.UserName).ToList(), Is.EquivalentTo(new[] { "BeboBryan", "TreesAreGreen", "YinWang" }));
            Assert.That(response.Value.Select(m => m.FirstName).ToList(), Is.EquivalentTo(new[] { "Bryan", "Teresa", "Yin" }));
            Assert.That(response.Value.Select(m => m.LastName).ToList(), Is.EquivalentTo(new[] { "Ang", "Green", "Wang" }));
            Assert.That(response.Value.Select(m => m.Email).ToList(), Is.EquivalentTo(new[] { "BryanAng@Gmail.com", "GreenTrees@Yahoo.com", "YinWang@qq.com" }));


        }


    }
}
