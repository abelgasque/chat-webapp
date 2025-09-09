using MediatR;
using ChatWebApp.Application.Commands;
using ChatWebApp.Application.Shared;
using ChatWebApp.Domain.Services;

namespace ChatWebApp.Application.Handlers
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

            if (user is null)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas");
            }

            var token = _authService.GenerateJwtToken(user);

            var loginResult = new LoginResult
            {
                Token = token,
                UserName = user.Username
            };

            return Result<LoginResult>.Ok(loginResult, "Autenticado com sucesso");
        }
    }
}