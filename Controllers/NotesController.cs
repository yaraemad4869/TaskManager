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
    public class NotesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public NotesController(
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
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetNotes()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var notes = await _uow.Notes.GetAllByUserId(user.Id);

            if (!notes.Any())
            {
                return NotFound("No notes found");
            }

            return Ok(_mapper.Map<IEnumerable<NoteDTO>>(notes));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDTO>> GetNote(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var note = await _uow.Notes.GetOneByIdAndUserId(id, user.Id);
            if (note == null)
            {
                return NotFound("Note not found");
            }

            return Ok(_mapper.Map<NoteDTO>(note));
        }
        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDTO>> GetNotes(string name)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var note = await _uow.Notes.GetOneByNameAndUserId(name, user.Id);
            if (note == null)
            {
                return NotFound("No Notes Found");
            }

            return Ok(_mapper.Map<NoteDTO>(note));
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNote(int id, NoteDTO noteDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (await _uow.Notes.IsUnique(noteDTO.Title, user.Id))
            {
                ModelState.AddModelError("UniqueError", "Note name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingNote = await _uow.Notes.GetOneByIdAndUserId(id, user.Id);
            if (existingNote == null)
            {
                return NotFound("Note not found");
            }
            //if (id != existingNote.Id)
            //{
            //    return BadRequest("ID mismatch");
            //}
            _mapper.Map(noteDTO, existingNote);
            try
            {
                return Ok(await _uow.NotesCRUD.UpdateAsync(existingNote));
            }
            catch
            {
                return BadRequest("Failed to update note");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<NoteDTO>> AddNote(NoteDTO noteDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (await _uow.Notes.IsUnique(noteDTO.Title, user.Id))
            {
                ModelState.AddModelError("UniqueError", "Note name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var note = _mapper.Map<Note>(noteDTO);
            note.UserId = user.Id;
            var createdNote = await _uow.NotesCRUD.AddAsync(note);
            return CreatedAtAction(
        actionName: nameof(AddNote),
        routeValues: new { id = note.Id },
        value: noteDTO);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDTO>> DeleteNote(int id)
        {
            var user = await GetCurrentUserAsync();
            var note = await _uow.Notes.DeleteAsync(id, user.Id);

            if (note == null)
            {
                return NotFound("Note not found");
            }
            return Ok(_mapper.Map<NoteDTO>(note));
        }
        [HttpDelete("by-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoteDTO>> DeleteNote(NoteDTO noteDTO)
        {
            var user = await GetCurrentUserAsync();
            var note = await _uow.Notes.DeleteAsync(noteDTO.Title, user.Id);

            if (note == null)
            {
                return NotFound("Note not found");
            }
            return Ok(_mapper.Map<NoteDTO>(note));
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}