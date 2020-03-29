using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiBackend.Dto;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatController : Controller
    {
        private readonly FlatManagementContext _context;
        private static readonly IMapper _MemberMapper;

        //Create mapper using Automapper
        static FlatController()
        {
            var mapperConfigure = new MapperConfiguration(
                config =>
                {
                    config.CreateMap<User, DisplayMemberDTO>();
                    config.CreateMap<User, UserDTO>();
                }

            );
            _MemberMapper = mapperConfigure.CreateMapper();
        }


        public FlatController(FlatManagementContext context)
        {
            _context = context;
        }


        [Authorize]
        [HttpGet("AddUserToFlat/{userName}")]    
        public ActionResult<AddUserToFlatDto> AddUserToFlat( string userName)
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            //the operating user and his flat
            var user = _context.User.Find(userID);
            Flat flat = _context.Flat.Where(f => f.Id == user.FlatId).FirstOrDefault();

            //the user who is operated
            User finduser = _context.User.Where(u => u.UserName == userName).FirstOrDefault();

            UserDTO ret = new UserDTO();
            ret = _MemberMapper.Map<User,UserDTO>(finduser);

            //user not exist
            if (finduser == null)
                return new AddUserToFlatDto(ret, 2);


            //user already in this flat 
            if (flat.Users.Contains(finduser))
                return new AddUserToFlatDto(ret, 4);

            //user has already in other flat  
            if (finduser.FlatId != null)
                return new AddUserToFlatDto(ret, 5);

            flat.Users.Add(finduser);
            finduser.FlatId = flat.Id;

            _context.Flat.Update(flat);
            _context.User.Update(finduser);
            _context.SaveChanges();

            return new AddUserToFlatDto(ret, 1);

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
            var user = _context.User.FirstOrDefault(x => x.Id == userID);
            var flat = _context.Flat.FirstOrDefault(fl => fl.Id == user.FlatId);
            if (flat != null)
            {
                _context.Entry(flat).Collection(fl => fl.Users).Load();
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
            var user = _context.User.FirstOrDefault(x => x.Id == userID);
            if (user.FlatId > 0)
            {
                return new ForbidResult();
            }
            var flat = new Flat
            {
                //Temporary flat address. Will need to be added in with the post request later
                Address = "50 Symonds Street",
                Users = new List<User> { user },
                Schedules = new List<Schedule>(),
                Payments = new List<Payment>()
            };
            user.Flat = flat;
            _context.Add(flat);
            _context.SaveChanges();
            Response.StatusCode = 201;
            return new FlatDTO(flat);
        }
    }
}