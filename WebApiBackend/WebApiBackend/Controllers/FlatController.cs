using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                }

            );
            _MemberMapper = mapperConfigure.CreateMapper();
        }


        public FlatController(FlatManagementContext context)
        {
            _context = context;
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
                Response.StatusCode = 403;
                return null;
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

        /// <summary>
        /// Delete Method -Remove a member out of a flat
        /// </summary>
        /// <param name="deleteUserName">The username of a member being removed from the flat</param>
        /// <response code="202">The user have successfully remove her/himself from the flat, and be redirected to create flat page</response>
        /// <returns>The response body is the flat members </returns>

        [Authorize]
        [HttpDelete("{deleteUserName}")]
        public ActionResult<FlatDTO> RemoveMember(string deleteUserName)
        {
            ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;
            int userId = Int32.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
           
            User user = _context.User.Find(userId);
            User deleteUser = _context.User.FirstOrDefault(u => u.UserName == deleteUserName);
            Flat flat = _context.Flat.Include(f => f.Users).FirstOrDefault(f => f.Id == user.FlatId);

           
            if (deleteUser == null || !flat.Users.Contains(deleteUser))
            {

                return NotFound("OOPS, user not in the flat.");
            }

            flat.Users.Remove(deleteUser);
            _context.SaveChanges();
     
            //code 202 retrun when the user is removing her/himself out from the flat
            if (deleteUser.Id == userId)
            {
                //flat will be remove from system once empty 
                if (flat.Users.Count == 0)
                {
                    _context.Flat.Remove(flat);
                    _context.SaveChanges();
                    
                }
                Response.StatusCode = 202;
                return null;
            }
            //return the new member list of the flat
            return new FlatDTO(flat);
        }



    }
}