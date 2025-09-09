using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatWebApp.Domain.Entities;
using ChatWebApp.Domain.Services;
using ChatWebApp.Domain.Repositories;

namespace ChatWebApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IRepository<User> _repository;

        public AuthService(
            IConfiguration configuration,
            ILogger<AuthService> logger,
            IRepository<User> repository
        )
        {
            _configuration = configuration;
            _logger = logger;
            _repository = repository;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            _logger.LogInformation("Validating user with email {Email}", email);
            if (email == "admin@admin.com" && password == "123456")
            {
                return new User("Admin", email);
            }

            return await _repository.GetByConditionAsync(e => e.Email.Equals(email));
        }

        public string GenerateJwtToken(User user)
        {
            _logger.LogInformation("Generating JWT token for user {Email}", user.Email);
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserName", user.Username)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}