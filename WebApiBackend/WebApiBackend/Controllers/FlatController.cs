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

        [Authorize]
        [HttpGet("display")]
        public ActionResult<List<DisplayMemberDTO>> GetMembers()
        {
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _context.User.Find(userID);
            Flat flat = _context.Flat.Where(f => f.Id == user.FlatId).FirstOrDefault();
            IQueryable members = _context.Entry(flat).Collection(f => f.Users).Query().OrderBy(u => u.FirstName);
            return _MemberMapper.Map<List<DisplayMemberDTO>>(members);
        }

    
        //Delete:api/delete/{username}
        [Authorize]
        [HttpDelete("{deleteUserName}")]
        public ActionResult RemoveMember(string deleteUserName)
        {
            ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;
            int userId = Int32.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = _context.User.Find(userId);
            User deleteUser = _context.User.Where(u => u.UserName == deleteUserName).First();
            Flat flat = _context.Flat.Include(f => f.Users).First(f => f.Id == user.FlatId);
 
            var redirectToActionResult = new RedirectToActionResult("createFlat", "Flat", null);
            flat.Users.Remove(deleteUser);
            _context.SaveChanges();
     
            if (deleteUser.Id == userId)
            {
                if (flat.Users.Count == 0)
                {
                    _context.Flat.Remove(flat);
                    _context.SaveChanges();
                }
                return redirectToActionResult;
            }

            return Ok(deleteUser.UserName + " has been removed from the flat.");
        }
    }
}