using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiBackend.Dto;
using WebApiBackend.Model;
using WebApiBackend.Helpers;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections.Generic;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly FlatManagementContext _database;

        public MembersController(IOptions<AppSettings> appSettings, FlatManagementContext context)
        {
            _appSettings = appSettings.Value;
            _database = context;
        }

        /// <summary>
        /// GET Method - Get a list of members in the current users flat
        /// </summary>
        /// <response code="200">Able to find the user are retrieve members of their flat</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns>Retrieves an array consisting of all the users in the current users flat in the form {"flatMembers":[{"member1}, {member2} ...]}. If the user is not part of a flat, returns an empty array</returns>
        [HttpGet("getMembers")]
        [Authorize]
        public ActionResult<FlatDTO> GetFlatMembers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _database.User.FirstOrDefault(x => x.Id == userID);
            var flat = _database.Flat.FirstOrDefault(fl => fl.Id == user.FlatId);
            if(flat != null) {
                _database.Entry(flat).Collection(fl => fl.Users).Load();
            }
            return new FlatDTO(flat);
        }

        /// <summary>
        /// GET Method - Get a list of members in the current users flat
        /// </summary>
        /// <response code="201">Able to find the user and create a new flat for them</response>
        /// <response code="401">Not an authorised user</response>
        /// /// <response code="403">Attempting to create a new flat while already part of one</response>
        /// <returns>The JSON representation of the users newly created flat</returns>
        [HttpPost("createFlat")]
        [Authorize]
        public ActionResult<FlatDTO> createFlat()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _database.User.FirstOrDefault(x => x.Id == userID);
            if (user.FlatId > 0)
            {
                Response.StatusCode = 403;
                return null;
            }
            var flat = new Flat
            {
                //Temporary flat address. Will need to be added in with the post request later
                Address = "50 Symonds Street",
                Users = new List<User> { user },
                Schedules = new List<Schedule> (),
                Payments = new List<Payment> ()
            };
            user.Flat = flat;
            _database.Add(flat);
            _database.SaveChanges();
            Response.StatusCode = 201;
            return new FlatDTO(flat);
        }
    }
}
