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
            var identity =(ClaimsIdentity) HttpContext.User.Identity;
            var username= identity.FindFirst(ClaimTypes.Name).Value;
            var user = _context.User.Find(username);
            Flat flat = _context.Flat.Where(f => f.Id ==user.FlatId).FirstOrDefault();
            IQueryable members = _context.Entry(flat).Collection(f => f.Users).Query().OrderBy(u => u.FirstName);
            return _MemberMapper.Map<List<DisplayMemberDTO>>(members);
        }

    }
}
