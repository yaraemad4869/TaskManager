using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data.Context;
using TaskManager.Data.Models;
using TaskManager.IRepositories;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _uow;

        public ToDoesController(AppDbContext context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        // GET: api/ToDoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>?> GetToDos()
        {
            List<ToDo>? toDos = await _uow.ToDos.GetAllAsync();
            if (toDos.Any())
                return Ok(toDos);
            return NotFound("No To-Dos");
        }

        // GET: api/ToDoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>?> GetToDo(int id)
        {
            var toDo = await _uow.ToDos.GetByIdAsync(id);

            if (toDo == null)
            {
                return NotFound("To-Do Not Found");
            }

            return Ok(toDo);
        }

        // PUT: api/ToDoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, ToDo toDo)
        {
            if(!ModelState.IsValid || id != toDo.Id)
            {
                ModelState.AddModelError("InvalidData", "Invalid Data");
                return BadRequest("Invalid Data");
            }

            try
            {
                ToDo toDo1= await _uow.ToDos.UpdateAsync(toDo);
                if (toDo1 == null)
                {
                    return NotFound("To-Do Not Found");
                }
                return Ok(toDo);
            }
            catch (DbUpdateConcurrencyException)
            {
                
            return NoContent();
            }

        }

        // POST: api/ToDoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostToDo(ToDo toDo)
        {
            if(ModelState.IsValid){
                await _uow.ToDos.AddAsync(toDo);

                return CreatedAtAction("GetToDo", new { id = toDo.Id }, toDo); 
            }

            ModelState.AddModelError("InvalidData", "Invalid Data");
            return BadRequest("Invalid Data");
        }

        // DELETE: api/ToDoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id)
        {
            var toDo = await _uow.ToDos.DeleteAsync(id);
            if (toDo == null)
            {
                return NotFound("To-Do Not Found");
            }
            return Ok(toDo);
        }

        //private bool ToDoExists(int id)
        //{
        //    return _context.ToDos.Any(e => e.Id == id);
        //}
    }
}
