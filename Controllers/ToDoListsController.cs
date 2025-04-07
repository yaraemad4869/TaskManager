using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.DTOs;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoListsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ToDoListsController(
            IUnitOfWork uow,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ToDoListDTO>>> GetToDoLists()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var todolists = await _uow.ToDoLists.GetAllByUserId(user.Id);

            if (!todolists.Any())
            {
                return NotFound("No todolists found");
            }

            return Ok(_mapper.Map<IEnumerable<ToDoListDTO>>(todolists));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoListDTO>> GetToDoList(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var todolist = await _uow.ToDoLists.GetOneByIdAndUserId(id, user.Id);
            if (todolist == null)
            {
                return NotFound("To-Do List not found");
            }

            return Ok(_mapper.Map<ToDoListDTO>(todolist));
        }
        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoListDTO>> GetToDoList(string name)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var todolist = await _uow.ToDoLists.GetOneByNameAndUserId(name, user.Id);
            if (todolist == null)
            {
                return NotFound("No To-Do Lists Found");
            }

            return Ok(_mapper.Map<ToDoListDTO>(todolist));
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateToDoList(int id, ToDoListDTO todolistDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var existingToDoList = await _uow.ToDoLists.GetOneByIdAndUserId(id, user.Id);
            if (existingToDoList == null)
            {
                return NotFound("ToDoList not found");
            }
            //if (id != existingToDoList.Id)
            //{
            //    return BadRequest("ID mismatch");
            //}
            if (await _uow.ToDoLists.IsUnique(todolistDTO.Title, user.Id))
            {
                ModelState.AddModelError("UniqueError", "To-Do List name already exists");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(todolistDTO,existingToDoList);
            try
            {
                return Ok(await _uow.ToDoListsCRUD.UpdateAsync(existingToDoList));
            }
            catch
            {
                return BadRequest("Failed to update To-do list");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToDoListDTO>> AddToDoList(ToDoListDTO todolistDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (await _uow.ToDoLists.IsUnique(todolistDTO.Title, user.Id))
            {
                ModelState.AddModelError("UniqueError", "To-Do List name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todolist = _mapper.Map<ToDoList>(todolistDTO);
            todolist.UserId = user.Id;
            var createdToDoList = await _uow.ToDoListsCRUD.AddAsync(todolist);
            return CreatedAtAction(
        actionName: nameof(AddToDoList),
        routeValues: new { id = todolist.Id },
        value: todolistDTO);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoListDTO>> DeleteToDoList(int id)
        {
            var user = await GetCurrentUserAsync();
            var todolist = await _uow.ToDoLists.DeleteAsync(id, user.Id);

            if (todolist == null)
            {
                return NotFound("To-Do List not found");
            }
            return Ok(_mapper.Map<ToDoListDTO>(todolist));
        }
        [HttpDelete("by-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoListDTO>> DeleteToDoList(ToDoListDTO todolistDTO)
        {
            var user = await GetCurrentUserAsync();
            var todolist = await _uow.ToDoLists.DeleteAsync(todolistDTO.Title, user.Id);

            if (todolist == null)
            {
                return NotFound("To-Do List not found");
            }
            return Ok(_mapper.Map<ToDoListDTO>(todolist));
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}