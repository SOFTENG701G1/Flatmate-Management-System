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
            var user = _database.User.First(user => user.UserName == username);
            var flat = _database.Flat.First(flat => flat.Users.Contains(user));
            return new FlatDTO(flat);
        }

        [HttpPost("createFlat")]
        [Authorize]
        public ActionResult<FlatDTO> createFlat()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var username = identity.FindFirst(ClaimTypes.Name).Value;
            var user = _database.User.First(user => user.UserName == username);
            var flat = new Flat();
            flat.Users = new System.Collections.Generic.List<User> { user };
            _database.Add(flat);
            Response.StatusCode = 201;
            return new FlatDTO(flat);
        }
    }
}
