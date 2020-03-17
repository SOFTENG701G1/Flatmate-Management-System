using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using WebApiBackend;
using WebApiBackend.Helpers;
using WebApiBackend.Model;

namespace WebApiBackendTests.Helper
{
    class DatabaseSetUpHelper
    {
        private readonly ServiceDependencyResolver _serviceProvider;
        private readonly FlatManagementContext _context;

        public DatabaseSetUpHelper()
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

        public FlatManagementContext GetContext()
        {
            return _context;
        }

        public ServiceDependencyResolver GetServiceDependencyResolver()
        {
            return _serviceProvider;
        }
    }
}
