using MediatR;
using ChatWebApp.Application.Shared;

namespace ChatWebApp.Application.Commands
{
    public class LoginCommand : IRequest<Result<LoginResult>>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}