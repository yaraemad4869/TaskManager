using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    //private readonly SignInManager<User> _signInManager;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthController(
        UserManager<User> userManager,
        //SignInManager<User> signInManager,
        TokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        //_signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDTO model)
    {
        
        if(ModelState.IsValid)
        {
            var user = _mapper.Map<User>(model);
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("Token");
                //return Ok(new { Token = _tokenService.GenerateToken(user) });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("RegistrationError", error.Description);
                }
            }
            //return BadRequest(result.Errors);
        }
        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        if(ModelState.IsValid){
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("LoginError", "Invalid Email.");

            }
            else
            {

                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var tokenResponse = await _tokenService.GenerateToken(user);
                    return Ok(tokenResponse);
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Invalid Password.");
                }

            }
        }
        return Unauthorized(ModelState);
    }
}