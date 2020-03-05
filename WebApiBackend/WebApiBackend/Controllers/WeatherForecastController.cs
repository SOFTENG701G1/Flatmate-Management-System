using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiBackend.Dto;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly FlatManagementContext _database;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, FlatManagementContext databaseContext)
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
    }
}
