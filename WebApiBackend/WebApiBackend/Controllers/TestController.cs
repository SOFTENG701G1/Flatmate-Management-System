using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiBackend.Dto;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly FlatManagementContext _database;

        public TestController(ILogger<TestController> logger, FlatManagementContext databaseContext)
        {
            _logger = logger;
            _database = databaseContext;
        }

        [HttpGet]
        public IEnumerable<TestItemDto> TestDatabaseGet() {
            return _database.TestItems.ToList().Select(item => new TestItemDto { 
                Id = item.Id, 
                Name = item.Name 
            });
        }

        [HttpGet("secure/")]
        [Authorize]
        public string TestAuthorization()
        {
            return "Ok";
        }
    }
}
