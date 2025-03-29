using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        //private readonly AppDbContext _context;

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _userId;
        public CategoriesController(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _mapper = mapper;
            _userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            List<Category>? categories = await _uow.Categories.GetAllAsync();
            if (categories.Any())
                return Ok(categories);
            return NotFound("No Categories");
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category? category = await _uow.Categories.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound("Category Not Found");
            }

            return Ok(category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            Category category = _mapper.Map<Category>(categoryDTO);
            if (id != category.Id)
            {
                return BadRequest("Category Not Found");
            }

            try
            {
                if(ModelState.IsValid){
                    return Ok(await _uow.Categories.UpdateAsync(category));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryDTO categoryDTO)
        {
            Category category = _mapper.Map<Category>(categoryDTO);
            if (ModelState.IsValid)
            {
                await _uow.Categories.AddAsync(category);
                return CreatedAtAction("GetCategory", new { id = category.Id }, category);
            }
            return BadRequest("Invalid Data");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Category? category = await _uow.Categories.DeleteAsync(id);
            if (category == null)
            {
                return NotFound("Category Not Found");
            }
            return Ok(category);
        }

        //private bool CategoryExists(int id)
        //{
        //    return _context.Categories.Any(e => e.Id == id);
        //}
    }
}
