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
    public class ColorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IUnitOfWork _uow;

        public ColorsController(AppDbContext context, IUnitOfWork uow)
        {
            _context = context;
            _uow = uow;
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<List<Color>>> GetColors()
        {
            List<Color>? colors = await _uow.Colors.GetAllAsync();
            if (colors.Any())
                return Ok(colors);
            return NotFound("No Colors");
        }

        // GET: api/Colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> GetColor(int id)
        {
            Color? color = await _uow.Colors.GetByIdAsync(id);

            if (color == null)
            {
                return NotFound("Color Not Found");
            }

            return Ok(color);
        }

        // PUT: api/Colors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColor(int id, Color color)
        {
            if (id != color.Id)
            {
                return BadRequest("Color Not Found");
            }

            _context.Entry(color).State = EntityState.Modified;

            try
            {
                return Ok(await _uow.Colors.UpdateAsync(color));
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }

        // POST: api/Colors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Color>> PostColor(Color color)
        {
            if (ModelState.IsValid)
            {
                await _uow.Colors.AddAsync(color);

                return CreatedAtAction("GetColor", new { id = color.Id }, color);
            }
            return BadRequest("Invalid Data");

        }

        // DELETE: api/Colors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            Color? color = await _uow.Colors.DeleteAsync(id);
            if (color == null)
            {
                return NotFound("Color Not Found");
            }
            return Ok(color);
        }

        //private bool ColorExists(int id)
        //{
        //    return _context.Colors.Any(e => e.Id == id);
        //}
    }
}
