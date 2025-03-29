using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Auth.Abstracts;
using TaskManager.Core.Models;
using TaskManager.DTOs;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userRegisterDto)
        {
            try
            {
                var userDto = await _authService.Register(userRegisterDto);
                return Ok(userDto);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO userLoginDto)
        {
            try
            {
                var userDto = await _authService.Login(userLoginDto);
                var token = _authService.GenerateJwtToken(_mapper.Map<User>(userDto));
                return Ok(new { user = userDto, token });
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
