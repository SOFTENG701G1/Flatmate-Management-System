using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiBackend.Interfaces;

namespace WebApiBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TRepository, TEntityDTO> : ControllerBase
        where TEntityDTO : class, IEntity
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;
        private readonly IMapper _mapper;

        public BaseController(TRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        // GET: api/[controller]
        /// <summary>
        /// FOR DEV TESTING
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntityDTO>>> Get()
        {
            List<TEntityDTO> entityDTOs = _mapper.Map<List<TEntity>, List<TEntityDTO>>(await _repository.GetAll());

            return Ok(entityDTOs);
        }

        // GET: api/[controller]/5
        /// <summary>
        /// FOR DEV TESTING
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntityDTO>> Get(int id)
        {
            var entity = await _repository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            TEntityDTO entityDTO = _mapper.Map<TEntity, TEntityDTO>(entity);

            return Ok(entityDTO);
        }

        // PUT: api/[controller]/5
        /// <summary>
        /// Update Object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityDTO"></param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TEntityDTO entityDTO)
        {
            if (id != entityDTO.Id)
            {
                return BadRequest();
            }

            TEntity entity = _mapper.Map<TEntityDTO, TEntity>(entityDTO);
            await _repository.Update(entity);

            return NoContent();
        }

        // POST: api/[controller]
        /// <summary>
        /// FOR DEV TESTING
        /// </summary>
        /// <param name="entityDTO"></param>
        /// <returns></returns>
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TEntityDTO>> Post(TEntityDTO entityDTO)
        {
            TEntity entity = _mapper.Map<TEntityDTO,TEntity>(entityDTO);
            await _repository.Add(entity);

            return CreatedAtAction("Get", new { id = entityDTO.Id }, entityDTO);
        }
    }
}
