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

        [HttpGet("AddUserToFlat/{FlatId}/{userId}")]
        [Authorize]
        public ActionResult<AddUserToFlatDto> AddUserToFlat(int FlatId, int userId)
        {

            User finduser = _context.User.Find(userId);

            //user not exist
            if (finduser == null)
                return new AddUserToFlatDto(FlatId,
                    userId,
                    2);

            Flat findflat = _context.Flat.Find(FlatId);

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
            _context.Flat.Update(findflat);
            _context.SaveChanges();

            return new AddUserToFlatDto(FlatId, userId, 1);

        }


    }
}