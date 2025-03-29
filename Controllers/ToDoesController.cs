using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Data;
using TaskManager.DTOs;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _userId;
        public ToDoesController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
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
        public async Task<IActionResult> UpdateToDo(int id, ToDoDTO toDoDTO)
        {
            ToDo toDo = _mapper.Map<ToDo>(toDoDTO);
            if (!ModelState.IsValid || id != toDo.Id)
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
        public async Task<ActionResult<ToDo>> AddToDo(ToDoDTO toDoDTO)
        {
            ToDo toDo = _mapper.Map<ToDo>(toDoDTO);
            if (ModelState.IsValid){
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
    }
}
