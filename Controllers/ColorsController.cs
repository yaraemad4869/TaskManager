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
    public class ColorsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public ColorsController(
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
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetColors()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var colors = await _uow.ColorsRepo.GetAllByUserId(user.Id);
            if (!colors.Any())
            {
                return NotFound("No colors found");
            }
            return Ok(_mapper.Map<IEnumerable<ColorDTO>>(colors));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ColorDTO>> GetColor(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var color = await _uow.ColorsRepo.GetOneByIdAndUserId(id, user.Id);
            if (color == null)
            {
                return NotFound("Color not found");
            }
            return Ok(_mapper.Map<ColorDTO>(color));
        }
        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetColor(string name)
        {
            var user = await GetCurrentUserAsync();
            var colors = await _uow.ColorsRepo.GetOneByNameAndUserId(name, user.Id);
            if (colors == null)
            {
                return NotFound("No Colors Found");
            }
            return Ok(_mapper.Map<ColorDTO>(colors));
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateColor(int id, ColorDTO colorDTO)
        {
            var user = await GetCurrentUserAsync();
            if (await _uow.ColorsRepo.IsUnique(colorDTO.Name, user.Id))
            {
                ModelState.AddModelError("UniqueError", "Color name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingColor = await _uow.ColorsRepo.GetOneByIdAndUserId(id, user.Id);
            if (existingColor == null)
            {
                return NotFound("Color not found");
            }
            //if (id != existingColor.Id)
            //{
            //    return BadRequest("ID mismatch");
            //}
            _mapper.Map(colorDTO, existingColor);
            try
            {
                await _uow.Colors.UpdateAsync(existingColor);
                return Ok(colorDTO);
            }
            catch(Exception e)
            {
                return BadRequest($"Failed to update color, {e}");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ColorDTO>> AddColor(ColorDTO colorDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (await _uow.ColorsRepo.IsUnique(colorDTO.Name, user.Id))
            {
                ModelState.AddModelError("UniqueError", "Color name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var color = _mapper.Map<Color>(colorDTO);
            color.UserId = user.Id;
            var createdColor = await _uow.Colors.AddAsync(color);
            return CreatedAtAction(
        actionName: nameof(AddColor),
        routeValues: new { id = color.Id },
        value: colorDTO);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ColorDTO>> DeleteColor(int id)
        {
            var user = await GetCurrentUserAsync();
            var color = await _uow.ColorsRepo.DeleteAsync(id, user.Id);

            if (color == null)
            {
                return NotFound("Color not found");
            }
            return Ok(_mapper.Map<ColorDTO>(color));
        }
        [HttpDelete("by-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ColorDTO>> DeleteColor(ColorDTO colorDTO)
        {
            var user = await GetCurrentUserAsync();
            var color = await _uow.ColorsRepo.DeleteAsync(colorDTO.Name, user.Id);

            if (color == null)
            {
                return NotFound("Color not found");
            }
            return Ok(_mapper.Map<ColorDTO>(color));
        }
        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}