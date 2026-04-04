using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ExamSystem.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IConfiguration _configuration;


        public UserService(IUnitOfWork unitofwork, IConfiguration configuration)
        {
            _unitofwork = unitofwork;
            _configuration = configuration;
        }
        public async Task<JwtSecurityToken> CreateTokenAsync(User user)
        {
            var role = await _unitofwork.Roles.GetByIdAsync(user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, role.Name.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signingCredentials = new SigningCredentials(
                                    key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: signingCredentials
            );
            return token;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _unitofwork.Users.GetByEmailAsync(dto.Email);
            if (user == null) 
            { 
                throw new UnauthorizedAccessException("Invalid email or password");
            }
            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = await CreateTokenAsync(user);
            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo
            };
        }
        

    }

}
