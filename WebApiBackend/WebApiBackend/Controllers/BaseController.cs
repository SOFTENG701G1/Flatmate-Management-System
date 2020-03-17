using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TRepository, TEntityDTO> : ControllerBase
        where TEntityDTO : class, IEntity
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository repository;
        private readonly IMapper mapper;

        public BaseController(TRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntityDTO>>> Get()
        {
            List<TEntityDTO> entityDTOs = mapper.Map<List<TEntity>, List<TEntityDTO>>(await repository.GetAll());

            return Ok(entityDTOs);
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntityDTO>> Get(int id)
        {
            var entity = await repository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            TEntityDTO entityDTO = mapper.Map<TEntity, TEntityDTO>(entity);

            return Ok(entityDTO);
        }

        // PUT: api/[controller]/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TEntityDTO entityDTO)
        {
            if (id != entityDTO.Id)
            {
                return BadRequest();
            }

            TEntity entity = mapper.Map<TEntityDTO, TEntity>(entityDTO);
            await repository.Update(entity);

            return NoContent();
        }

        // POST: api/[controller]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TEntityDTO>> Post(TEntityDTO entityDTO)
        {
            TEntity entity = mapper.Map<TEntityDTO,TEntity>(entityDTO);
            await repository.Add(entity);

            return CreatedAtAction("Get", new { id = entityDTO.Id }, entityDTO);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntityDTO>> Delete(int id)
        {
            var entity = await repository.Delete(id);
            if (entity == null)
            {
                return NotFound();
            }
            TEntityDTO entityDTO = mapper.Map<TEntity, TEntityDTO>(entity);


            return entityDTO;
        }
    }
}
