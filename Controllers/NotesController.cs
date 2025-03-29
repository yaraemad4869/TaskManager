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
    public class NotesController : ControllerBase
    {

        private readonly IUnitOfWork _uow;
        private readonly IItemRepo<Note> _noteRepo;

        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _userId;
        public NotesController(IUnitOfWork uow, IItemRepo<Note> noteRepo , IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _noteRepo = noteRepo;
            _userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            List<Note>? notes = await _uow.Notes.GetAllAsync();
            if(notes.Any())
                return Ok(notes);
            return NotFound("No Notes");
        }
        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<Note>> GetByCategory(int categoryId)
        {
            User? user = await _uow.UsersRepo.GetUserByEmailAsync(User.Identity.Name);
            var notes = await _noteRepo.GetWithCategory(user, categoryId);

            if (notes == null)
            {
                return NotFound("No To-Do Lists");
            }

            return Ok(notes);
        }
        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            Note? note = await _uow.Notes.GetByIdAsync(id);

            if (note == null)
            {
                return NotFound("Note Not Found");
            }

            return Ok(note);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, NoteDTO noteDTO)
        {
            Note note = _mapper.Map<Note>(noteDTO);
            if (id != note.Id)
            {
                return BadRequest("Note Not Found");
            }

            try
            {
                return Ok(await _uow.Notes.UpdateAsync(note));
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Note>> AddNote(NoteDTO noteDTO)
        {
            Note note = _mapper.Map<Note>(noteDTO);
            if (ModelState.IsValid)
            {
                await _uow.Notes.AddAsync(note);

                return CreatedAtAction("GetNote", new { id = note.Id }, note);
            }
            return BadRequest("Invalid Data");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            Note? note = await _uow.Notes.DeleteAsync(id);
            if (note == null)
            {
                return NotFound("Note Not Found");
            }
            return Ok(note);
        }

        //private bool NoteExists(int id)
        //{
        //    return _context.Notes.Any(e => e.Id == id);
        //}
    }
}
