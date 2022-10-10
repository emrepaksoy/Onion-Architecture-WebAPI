using E_TicaretAPI.Application.Abstraction.Services;
using E_TicaretAPI.Application.DTOs.User;
using E_TicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace E_TicaretAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {

        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                UserName = request.UserName,

            });

            return new() 
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };
        }
    }
}
// ASP.NET Core Identity mekanızmasında --> UserManager servisi kullanıcı ekleme servisi
//IoC e AddIdentity eklendikten sonra talep  edilebiler.