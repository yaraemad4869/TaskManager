using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Auth.Abstracts;
using TaskManager.Core.Enums;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.DTOs;

namespace TaskManager.Auth.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> Register(UserDTO registerDto)
        {
            if (await _unitOfWork.UsersRepo.EmailExistsAsync(registerDto.Email))
            {
                throw new ApplicationException("Email already exists");
            }
            //var currentUser = await GetCurrentUserAsync();
            var user = _mapper.Map<User>(registerDto);
            //if (user.userType > UserType.User && (currentUser == null || currentUser.userType < UserType.Admin))
            //{
            //    throw new ApplicationException("Insufficient privileges to create this user type");
            //}
            user.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            await _unitOfWork.Users.AddAsync(user);

            return user;
        }

        public async Task<User> Login(LoginDTO loginDto)
        {
            var user = await _unitOfWork.UsersRepo.VerifyUser(loginDto);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new ApplicationException("Invalid email or password");
            }

            return user;
        }
        private async Task<User> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return null;

            return await _unitOfWork.Users.GetByIdAsync(int.Parse(userId));
        }
        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("UserType", user.userType.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(21),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
