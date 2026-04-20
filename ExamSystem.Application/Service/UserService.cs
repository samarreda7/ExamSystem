using ExamSystem.Application.DTO;
using ExamSystem.Application.IService;
using ExamSystem.Domain.IRepository;
using ExamSystem.Domain.Models;
using ExamSystem.Domain.ValueTypes;
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

        public async Task<ShowCurrentUserDto> GetCurrentUserAsync(Guid userId)
        {
            var user = await _unitofwork.Users.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"There is no user with Id: {userId}");
            }

            var role = await _unitofwork.Roles.GetByIdAsync(user.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException("Role not found");
            }

            return new ShowCurrentUserDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Role = role.Name
            };
        }


        public async Task<bool> IsAdminExistsAsync()
        {
            var adminRole = await _unitofwork.Roles.GetRoleName(RoleName.Admin.ToString());

            if (adminRole == null) return false;

           
            var users = await _unitofwork.Users.GetAllAsync();
            return users.Any(u => u.RoleId == adminRole.Id);
        }

   
        public async Task InitializeAdminAsync(InitAdminDto dto)
        {
            
            var adminRole = await _unitofwork.Roles
                                .GetRoleName(RoleName.Admin.ToString());
            if (adminRole == null)
                throw new KeyNotFoundException("Admin role not found.");

            var users = await _unitofwork.Users.GetAllAsync();
            bool adminExists = users.Any(u => u.RoleId == adminRole.Id);
            if (adminExists)
                throw new InvalidOperationException("Setup already completed.");

            var passwordHasher = new PasswordHasher<User>();
            var adminUser = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = adminRole.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, dto.Password);

            await _unitofwork.Users.AddAsync(adminUser);
            await _unitofwork.SaveChangesAsync();
        }
    }

}
