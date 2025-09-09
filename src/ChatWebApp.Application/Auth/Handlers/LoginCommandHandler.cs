using MediatR;
using ChatWebApp.Application.Auth.Commands;
using ChatWebApp.Application.Shared;
using ChatWebApp.Domain.Services;

namespace ChatWebApp.Application.Auth.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResult>>
    {
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Result<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _authService.ValidateUserAsync(request.Email, request.Password);
            var token = _authService.GenerateJwtToken(user);

            return Result<LoginResult>.Ok(new LoginResult
            {
                Token = token,
                UserName = user.Username
            }, "Autenticado com sucesso");
        }
    }
}