using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiBackend.Dto;
using WebApiBackend.Helpers;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlatController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly FlatManagementContext _database;

        public FlatController(IOptions<AppSettings> appSettings, FlatManagementContext databaseContext)
        {
            _appSettings = appSettings.Value;
            _database = databaseContext;
        }

        // One API Route Sample
        //[HttpGet]
        //public List<User> ReturnGet()
        //{
        //    return _database.User.ToList();
        //}

        // GET /<controller>/username
        [HttpGet("AddUserToFlat/{FlatId}/{userId}")]
        [Authorize]
        public ActionResult<AddUserToFlatDto> AddUserToFlat(int FlatId,int userId)
        {
            
            User finduser = _database.User.Find(userId);

            //user not exist
            if (finduser == null)
                return new AddUserToFlatDto(FlatId, 
                    userId, 
                    2);

            Flat findflat = _database.Flat.Find(FlatId);

            //wrong flat id
            if (findflat == null)
                return new AddUserToFlatDto(FlatId, 
                    userId, 
                    3);

            //user already in this flat 
            if (findflat.Users.Contains(finduser))
                return new AddUserToFlatDto(
                    FlatId, 
                    userId, 
                    4);

            //user has already in other flat  
            if (finduser.FlatId != null)
                return new AddUserToFlatDto(
                    FlatId,
                    userId,
                    5);

            findflat.Users.Add(finduser);
            _database.Flat.Update(findflat);
            _database.SaveChanges();

            return new AddUserToFlatDto(FlatId, userId, 1);
            
        }

    }
}
