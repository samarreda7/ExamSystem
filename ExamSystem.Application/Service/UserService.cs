using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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
                new Claim(ClaimTypes.Role, role.Name.ToString()),
                new Claim("uid", user.Id.ToString())
            };
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT key is missing.");
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var signingCredentials = new SigningCredentials(
                                    key, SecurityAlgorithms.HmacSha256);
            var jwtIssuer = _configuration["Jwt:Issuer"];
            if (string.IsNullOrWhiteSpace(jwtIssuer))
            {
                throw new InvalidOperationException("JWT issuer is missing.");
            }

            var jwtAudience = _configuration["Jwt:Audience"];
            if (string.IsNullOrWhiteSpace(jwtAudience)) 
            { 
                throw new InvalidOperationException("JWT audience is missing.");
            }
            var jwtDurationValue = _configuration["Jwt:DurationInMinutes"];
            if (!double.TryParse(jwtDurationValue, out var jwtDurationMinutes))
            {
                throw new InvalidOperationException("JWT duration is invalid.");
            }
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtDurationMinutes),
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
            var role = await _unitofwork.Roles.GetByIdAsync(user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }
            var token = await CreateTokenAsync(user);
          
            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo,
                 Role = role.Name,   
                UserId = user.Id
            };
        }
        

    }

}
