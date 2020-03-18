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


    }
}