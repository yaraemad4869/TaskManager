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
    public class ColorsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _userId;
        public ColorsController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColor(int id, ColorDTO colorDTO)
        {
            Color color = _mapper.Map<Color>(colorDTO);
            if (id != color.Id)
            {
                return BadRequest("Color Not Found");
            }

            try
            {
                return Ok(await _uow.Colors.UpdateAsync(color));
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Color>> AddColor(ColorDTO colorDTO)
        {
            Color color = _mapper.Map<Color>(colorDTO);
            if (ModelState.IsValid)
            {
                await _uow.Colors.AddAsync(color);

                return CreatedAtAction("GetColor", new { id = color.Id }, color);
            }
            return BadRequest("Invalid Data");

        }
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
