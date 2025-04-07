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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CategoriesController(
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
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            var user = await GetCurrentUserAsync();
            if(user == null)
            {
                return RedirectToAction("Login","Auth");
            }
            var categories = await _uow.CategoriesRepo.GetAllByUserId(user.Id);

            if (!categories.Any())
            {
                return NotFound("No categories found");
            }

            return Ok(_mapper.Map<IEnumerable<CategoryDTO>>(categories));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var category = await _uow.CategoriesRepo.GetOneByIdAndUserId(id, user.Id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            return Ok(_mapper.Map<CategoryDTO>(category));
        }
        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetCategory(string name)
        {
            var user = await GetCurrentUserAsync();
            var categories = await _uow.CategoriesRepo.GetOneByNameAndUserId(name, user.Id);
            if (categories == null)
            {
                return NotFound("No Categories Found");
            }

            return Ok(_mapper.Map<CategoryDTO>(categories));
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, string name)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var existingCategory = await _uow.CategoriesRepo.GetOneByIdAndUserId(id, user.Id);
            if (existingCategory == null)
            {
                return NotFound("Category not found");
            }
            //if (id != existingCategory.Id)
            //{
            //    return BadRequest("ID mismatch");
            //}
            if (await _uow.CategoriesRepo.IsUnique(name, user.Id))
            {
                return BadRequest("Category name already exists");
            }
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            existingCategory.Name = name;
            try
            {
                await _uow.Categories.UpdateAsync(existingCategory);
                return Ok(name);
            }
            catch
            {
                return BadRequest("Failed to update category");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CategoryDTO categoryDTO)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if(await _uow.CategoriesRepo.IsUnique(categoryDTO.Name, user.Id))
            {
                ModelState.AddModelError("UniqueError", "Category name already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryDTO);
            category.UserId = user.Id;
            var createdCategory =await _uow.Categories.AddAsync(category);
            return CreatedAtAction(
        actionName: nameof(AddCategory),
        routeValues: new { id = category.Id },
        value: categoryDTO);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> DeleteCategory(int id)
        {
            var user = await GetCurrentUserAsync();
            var category = await _uow.CategoriesRepo.DeleteAsync(id, user.Id);

            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(_mapper.Map<CategoryDTO>(category));
        }
        [HttpDelete("by-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> DeleteCategory(CategoryDTO categoryDTO)
        {
            var user = await GetCurrentUserAsync();
            var category = await _uow.CategoriesRepo.DeleteAsync(categoryDTO.Name, user.Id);

            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(_mapper.Map<CategoryDTO>(category));
        }

        private async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}