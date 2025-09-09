using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChatWebApp.Application.Auth.Commands;

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
    }
}