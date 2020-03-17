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
    public class FlatController : ControllerBase
    {
        private readonly FlatManagementContext _context;


        public FlatController(FlatManagementContext context)
        {
            _context = context;
        }

 
        //Delete:api/delete/{username}
        [Authorize]
        [HttpDelete("{deleteUserName}")]
        public ActionResult RemoveMember(string deleteUserName)
        {
            ClaimsIdentity identity = (ClaimsIdentity)HttpContext.User.Identity;
            var userId = Int32.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = _context.User.Find(userId);
            User deleteUser = _context.User.Where(u => u.UserName == deleteUserName).FirstOrDefault();
            Flat flat = _context.Flat.Find(user.FlatId);

 
            var redirectToActionResult = new RedirectToActionResult("createFlat", "Flat", null);
            flat.Users.Remove(deleteUser);
            _context.SaveChanges();
     
            if (user.UserName == deleteUserName)
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

