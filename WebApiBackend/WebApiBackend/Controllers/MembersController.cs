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

        [HttpGet("getMembers")]
        [Authorize]
        public ActionResult<FlatDTO> GetFlatMembers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var username = identity.FindFirst(ClaimTypes.Name).Value;
            var user = _database.User.FirstOrDefault(x => x.UserName == username);
            var flat = _database.Flat.FirstOrDefault(fl => fl.Id == user.FlatId);
            if(flat != null) {
                _database.Entry(flat).Collection(fl => fl.Users).Load();
            }
            return new FlatDTO(flat);
        }

        [HttpPost("createFlat")]
        [Authorize]
        public ActionResult<FlatDTO> createFlat()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var username = identity.FindFirst(ClaimTypes.Name).Value;
            var user = _database.User.First(user => user.UserName == username);
            var flat = new Flat
            {
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
