using E_TicaretAPI.Application.Abstraction.Services;
using E_TicaretAPI.Application.DTOs.User;
using E_TicaretAPI.Application.Exceptions;
using E_TicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                NameSurname = model.NameSurname,
            }, model.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı Oluşturuldu.";
            else
            {
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            }

            return response;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, DateTime AccessTokenDate, int AddOnAccessToken)
        {
           
            if (user !=null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = AccessTokenDate.AddSeconds(AddOnAccessToken);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();

        }
    }
}
