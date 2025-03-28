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
    public class ToDoListsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IUnitOfWork _uow;
        public ToDoListsController(AppDbContext context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        // GET: api/ToDoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoList>>> GettoDoLists()
        {
            List<ToDoList>? toDoLists = await _uow.ToDoLists.GetAllAsync();
            if (toDoLists.Any())
                return Ok(toDoLists);
            return NotFound("No To-Do Lists");
        }

        // GET: api/ToDoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoList>> GetToDoList(int id)
        {
            var toDoList = await _uow.ToDoLists.GetByIdAsync(id);

            if (toDoList == null)
            {
                return NotFound("To-Do List Not Found");
            }

            return Ok(toDoList);
        }

        // PUT: api/ToDoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoList(int id, ToDoList toDoList)
        {
            if (id != toDoList.Id)
            {
                return BadRequest("To-Do List Not Found");
            }

            _context.Entry(toDoList).State = EntityState.Modified;

            try
            {
                return Ok(await _uow.ToDoLists.UpdateAsync(toDoList));
            }
            catch (DbUpdateConcurrencyException)
            {

                return NoContent();
            }
        }

        // POST: api/ToDoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoList>> PostToDoList(ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                await _uow.ToDoLists.AddAsync(toDoList);

                return CreatedAtAction("GetToDoList", new { id = toDoList.Id }, toDoList);
            }
            return BadRequest("Invalid Data");
        }

        // DELETE: api/ToDoLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoList(int id)
        {
            var toDoList = await _uow.ToDoLists.DeleteAsync(id);
            if (toDoList == null)
            {
                return NotFound("To-Do Not Found");
            }
            return Ok(toDoList);
        }

        //private bool ToDoListExists(int id)
        //{
        //    return _context.toDoLists.Any(e => e.Id == id);
        //}
    }
}
