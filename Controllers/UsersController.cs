using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;
using TaskManager.DTOs;
using TaskManager.Core.Models;

namespace TaskManager.Controllers
{
    // Controllers/UsersController.cs
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _unitOfWork.Users.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            //var currentUser = await _unitOfWork.Users.GetByIdAsync(_currentUserId);
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null) return NotFound();

            // Users can view their own profile, admins can view any profile
            //if (user.Id != _currentUserId && currentUser.userType < UserType.Admin)
            //{
            //    return Forbid();
            //}

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDTO userDto)
        {
            //var currentUser = await _unitOfWork.Users.GetByIdAsync(_currentUserId);

            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null) return NotFound();

            // Users can update their own profile, admins can update any profile
            //if (user.Id != _currentUserId && currentUser.userType < UserType.Admin)
            //{
            //    return Forbid();
            //}

            // Only admins can change UserType
            //if (user.Id != currentUser.Id && currentUser.userType < UserType.Admin)
            //{
            //    return Forbid();
            //}

            // Prevent downgrading higher-level users
            //if (user.userType > currentUser.userType)
            //{
            //    return Forbid();
            //}

            _mapper.Map(userDto, user);
            await _unitOfWork.Users.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            //var currentUser = await _unitOfWork.Users.GetByIdAsync(_currentUserId);
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null) return NotFound();

            // Users can delete their own account, admins can delete any account
            //if (user.Id != _currentUserId && currentUser.userType < UserType.Admin)
            //{
            //    return Forbid();
            //}

            // Prevent deleting higher-level users
            //if (user.userType > currentUser.userType)
            //{
            //    return Forbid();
            //}

            await _unitOfWork.Users.DeleteAsync(user);

            return NoContent();
        }
    }
}
