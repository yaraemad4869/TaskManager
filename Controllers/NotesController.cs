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
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IUnitOfWork _uow;

        public NotesController(AppDbContext context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            List<Note>? notes = await _uow.Notes.GetAllAsync();
            if(notes.Any())
                return Ok(notes);
            return NotFound("No Notes");
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

        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest("Note Not Found");
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                return Ok(await _uow.Notes.UpdateAsync(note));
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }

        // POST: api/Notes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            if (ModelState.IsValid)
            {
                await _uow.Notes.AddAsync(note);

                return CreatedAtAction("GetNote", new { id = note.Id }, note);
            }
            return BadRequest("Invalid Data");

        }

        // DELETE: api/Notes/5
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
