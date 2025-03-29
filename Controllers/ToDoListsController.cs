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
    public class ToDoListsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IItemRepo<ToDoList> _toDoListRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _userId;
        public ToDoListsController(IUnitOfWork uow, IItemRepo<ToDoList> toDoListRepo, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _toDoListRepo = toDoListRepo;
            _mapper = mapper;
            _userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
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

        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<ToDoList>> GetByCategory (int categoryId)
        {
            User? user = await _uow.UsersRepo.GetUserByEmailAsync(User.Identity.Name);
            var toDoLists = await _toDoListRepo.GetWithCategory(user,categoryId);

            if (toDoLists == null)
            {
                return NotFound("No To-Do Lists");
            }

            return Ok(toDoLists);
        }

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
        public async Task<IActionResult> UpdateToDoList(int id, ToDoListDTO toDoListDTO)
        {
            ToDoList toDoList = _mapper.Map<ToDoList>(toDoListDTO);
            if (id != toDoList.Id)
            {
                return BadRequest("To-Do List Not Found");
            }

            try
            {
                return Ok(await _uow.ToDoLists.UpdateAsync(toDoList));
            }
            catch (DbUpdateConcurrencyException)
            {

                return NoContent();
            }
        }
        [HttpPost]
        public async Task<ActionResult<ToDoList>> AddToDoList(ToDoListDTO toDoListDTO)
        {
            ToDoList toDoList = _mapper.Map<ToDoList>(toDoListDTO);
            if (ModelState.IsValid)
            {
                await _uow.ToDoLists.AddAsync(toDoList);

                return CreatedAtAction("GetToDoList", new { id = toDoList.Id }, toDoList);
            }
            return BadRequest("Invalid Data");
        }
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
