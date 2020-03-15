using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity,TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository repository;

        public BaseController(TRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await repository.GetAll();
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var entity = await repository.Get(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        // PUT: api/[controller]/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            await repository.Update(entity);

            return NoContent();
        }

        // POST: api/[controller]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            await repository.Add(entity);

            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TEntity>> Delete(int id)
        {
            var entity = await repository.Delete(id);
            if (entity == null)
            {
                return NotFound();
            }



            return entity;
        }
    }
}
