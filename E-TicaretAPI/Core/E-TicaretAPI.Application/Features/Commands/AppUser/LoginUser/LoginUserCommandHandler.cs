using E_TicaretAPI.Application.Abstraction.Services;
using E_TicaretAPI.Application.Abstraction.Token;
using E_TicaretAPI.Application.DTOs;
using E_TicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace E_TicaretAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {

            var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password,5);
            return new LoginUserSuccessCommandResponse()
            { 
                Token = token,
            };
        }
    }
}
