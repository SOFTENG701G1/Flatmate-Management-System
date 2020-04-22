using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoreController : BaseController<Chore, ChoresRepository, ChoreDTO>
    {
        private readonly ChoresRepository _choresRepository;
        private readonly FlatRepository _flatRepository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;

        public ChoreController(ChoresRepository choresRepository, FlatRepository flatRepository, UserRepository userRepository, IMapper mapper
            ) : base(choresRepository, mapper)
        {
            this._choresRepository = choresRepository;
            this._flatRepository = flatRepository;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// POST Method - Creates a new chore for the flat
        /// </summary>
        /// <param name="choreDTO">The chore you want to be created</param>
        /// <response code="200">Payment created</response>
        /// <response code="400">Asignee does not exist</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns> The created chore is returned </returns>        
        [HttpPost("Flat")]
        public async Task<IActionResult> CreateChoreForFlat([FromBody] ChoreDTO choreDTO)
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usr = await _userRepository.Get(userID);
            int? flatId = usr.FlatId;
            var assignee = await _userRepository.Get(choreDTO.Assignee);

            if (assignee == null) return BadRequest();

            Chore chore = _mapper.Map<ChoreDTO, Chore>(choreDTO);
            chore.AssignedUser = assignee;

            await _choresRepository.Add(chore);

            List<Chore> chores = await GetAllChoresFromFlatId(flatId);

            Flat flat = await _flatRepository.Get(flatId.Value);

            chores.Add(chore);
            flat.Chores = chores;
            await _flatRepository.Update(flat);

            return Ok();
        }


        /// <summary>
        /// GET Method - Gets all chores for a flat
        /// </summary>
        /// <response code="200">Chores for flat returned</response>
        /// <response code="400">Flat does not exist for signed in user</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns> The chores for a flat are returned </returns>        
        [HttpGet("Flat")]
        public async Task<IActionResult> GetAllChoresForFlat()
        {
            // Get flat ID from the user creds
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usr = await _userRepository.Get(userID);
            int? flatId = usr.FlatId;

            if (flatId == null)
            {
                return BadRequest();
            }

            List<Chore> chores = await GetAllChoresFromFlatId(flatId);

            List<ChoreDTO> dtos = new List<ChoreDTO>();

            foreach (Chore c in chores)
            {
                dtos.Add(new ChoreDTO(c));
            }

            return Ok(dtos);
        }

        /// <summary>
        /// DELETE Method - Deletes a chore based on its ID
        /// </summary>
        /// <response code="200">Chore has been deleted</response>
        /// <response code="400">Chore does not exist for signed-in user</response>
        /// <response code="401">Not an authorised user</response>
        [HttpDelete("{choreID}")]
        public async Task<IActionResult> DeleteChore([FromRoute] int choreID)
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usr = await _userRepository.Get(userID);
            int? flatId = usr.FlatId;

            var chore = await _choresRepository.Get(choreID);

            if (flatId == null || chore == null)
            {
                return BadRequest();
            }

            // Ensure the chore belongs to the same flat as the user
            List<Chore> chores = await GetAllChoresFromFlatId(flatId);
            if (!chores.Contains(chore))
            {
                return BadRequest();
            }

            await _choresRepository.Delete(choreID);
            return Ok();
        }

        /// <summary>
        /// Helper method to get all chores for a specific flat
        /// </summary>
        /// <param name="flatId"></param>
        /// <returns>List of chores</returns>
        private async Task<List<Chore>> GetAllChoresFromFlatId(int? flatId)
        {
            List<Chore> chores = await _choresRepository.GetAll();
            List<User> users = await _userRepository.GetAll();

            chores = (from c in chores
                      join u in users on c.AssignedUser.Id equals u.Id
                      where u.FlatId.Equals(flatId)
                        select c).Distinct().ToList();

            return chores;
        }

        /// <summary>
        /// PUT Method - sets specific chore to completed
        /// </summary>
        /// <response code="200">Chore successfully marked as completed</response>
        /// <response code="400">Bad request, chore does not exist</response>
        /// <response code="401">Not an authorised user</response>      
        [HttpPut("Chores/{choreId}")]
        public async Task<IActionResult> MarkChoreAsCompleted(int choreId)
        {
            //check identity
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _userRepository.Get(userID);
            if (user == null)
            {
                return new BadRequestResult();
            }

            Chore chore = await _choresRepository.Get(choreId);
            //check chore exists
            if (chore == null)
            {
                return BadRequest();
            }

            chore.Completed = !chore.Completed;
            await _choresRepository.Update(chore);

            return Ok();
        }

    }
}
