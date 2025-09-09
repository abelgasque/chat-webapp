using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChatWebApp.Application.Commands;

namespace ChatWebApp.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza o login do usuário.
        /// </summary>
        /// <param name="command">Email e senha do usuário.</param>
        /// <returns>Token JWT se as credenciais forem válidas.</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Credenciais inválidas</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}