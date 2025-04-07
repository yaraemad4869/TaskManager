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
    public class ToDoesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ToDoesController(
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
        public async Task<ActionResult<IEnumerable<ToDoDTO>>> GetToDoes()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var todos = await _uow.ToDoes.GetAllByUserId(user.Id);

            if (!todos.Any())
            {
                return NotFound("No todos found");
            }

            return Ok(_mapper.Map<IEnumerable<ToDoDTO>>(todos));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoDTO>> GetToDo(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var todo = await _uow.ToDoes.GetOneByIdAndUserId(id, user.Id);
            if (todo == null)
            {
                return NotFound("ToDo not found");
            }

            return Ok(_mapper.Map<ToDoDTO>(todo));
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateToDo(int id, ToDoDTO todoDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var existingToDo = await _uow.ToDoes.GetOneByIdAndUserId(id, user.Id);
            if (existingToDo == null)
            {
                return NotFound("ToDo not found");
            }
            if (id != existingToDo.Id)
            {
                return BadRequest("ID mismatch");
            }
            if (await _uow.ToDoes.IsUnique(todoDTO.Name, user.Id))
            {
                ModelState.AddModelError("UniqueError", "ToDo name already exists");
            }
            _mapper.Map(todoDTO,existingToDo);
            try
            {
                return Ok(await _uow.ToDoesCRUD.UpdateAsync(existingToDo));
            }
            catch
            {
                return BadRequest("Failed to update todo");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ToDoDTO>> AddToDo(ToDoDTO todoDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (await _uow.ToDoes.IsUnique(todoDTO.Name, user.Id))
            {
                ModelState.AddModelError("UniqueError", "ToDo name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todo = _mapper.Map<ToDo>(todoDTO);
            todo.toDoList.UserId = user.Id;
            var createdToDo = await _uow.ToDoesCRUD.AddAsync(todo);
            return CreatedAtAction(
        actionName: nameof(AddToDo),
        routeValues: new { id = todo.Id },
        value: todoDTO);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoDTO>> DeleteToDo(int id)
        {
            var user = await GetCurrentUserAsync();
            var todo = await _uow.ToDoes.DeleteAsync(id, user.Id);

            if (todo == null)
            {
                return NotFound("ToDo not found");
            }
            return Ok(_mapper.Map<ToDoDTO>(todo));
        }
        [HttpDelete("by-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ToDoDTO>> DeleteToDo(ToDoDTO todoDTO)
        {
            var user = await GetCurrentUserAsync();
            var todo = await _uow.ToDoes.DeleteAsync(todoDTO.Name, user.Id);

            if (todo == null)
            {
                return NotFound("ToDo not found");
            }
            return Ok(_mapper.Map<ToDoDTO>(todo));
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}