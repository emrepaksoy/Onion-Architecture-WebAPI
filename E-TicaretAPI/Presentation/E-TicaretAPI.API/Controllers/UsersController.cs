using E_TicaretAPI.Application.Features.Commands.AppUser.CreateUser;
using E_TicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using E_TicaretAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_TicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommandRequest createUserCommandRequest) => Ok( await _mediator.Send(createUserCommandRequest));


        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest) => Ok(await _mediator.Send(loginUserCommandRequest));
            
        

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromQuery] RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest) =>  Ok(await _mediator.Send(refreshTokenLoginCommandRequest));
           
        
    }

}
