using ChatWebApp.Domain.Entities;

namespace ChatWebApp.Domain.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        string GenerateJwtToken(User user);
    }
}