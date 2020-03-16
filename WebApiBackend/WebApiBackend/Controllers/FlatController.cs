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
    public class FlatController : ControllerBase
    {
        private readonly FlatManagementContext _context;


        public FlatController(FlatManagementContext context)
        {
            _context = context;
        }

        //TODO: Change AllowAnonymous to [Authorize] after integrate with front end
        //Delete:api/delete/{username}

        [AllowAnonymous]
        [HttpDelete("{deleteUserName}")]
        public ActionResult RemoveMember(string deleteUserName)
        {
            //ClaimsIdentity identity =(ClaimsIdentity) HttpContext.User.Identity;
            //var userName = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userName = "YinWang";
            User user = _context.User.Find(userName);
            User deleteUser = _context.User.Find(deleteUserName);
            Flat flat = _context.Flat.Find(user.FlatId);

            //TODO change param1 to newFlat later
            var redirectToActionResult = new RedirectToActionResult("RedirectTest", "Flat", null);
            var beforeCount = flat.Users.Count;
            flat.Users.Remove(deleteUser);
            var afterCount = flat.Users.Count;
            if (user.UserName == deleteUserName)
            {
                //if (flat.Users.Count == 0)
                //{
                //    _context.Flat.Remove(flat);
     
                //    deleteUser.Flat = null;
                //    deleteUser.FlatId = null;         
                //    _context.User.Update(deleteUser);
                //    _context.SaveChanges();
                //}
    //            return redirectToActionResult;
            }
            _context.SaveChanges();
            return Ok(deleteUser.UserName + " has been removed from the flat. Current member " + flat.Users.Count() + " before " + beforeCount + " after " + afterCount);
        }


        [AllowAnonymous]
        [HttpGet]
        public string RedirectTest()
        {
            return "redirect successfully";
        }




    }
}

