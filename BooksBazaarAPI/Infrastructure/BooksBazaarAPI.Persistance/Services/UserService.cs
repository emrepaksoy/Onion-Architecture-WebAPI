using BooksBazaarAPI.Application.Abstraction.Services;
using BooksBazaarAPI.Application.DTOs;
using BooksBazaarAPI.Application.Exceptions;
using BooksBazaarAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Persistance.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser userModel)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userModel.UserName,
                Email = userModel.Email,
                NameSurname=userModel.NameSurname,


            }, userModel.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı oluşturuldu.";
            else
            {
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            }


            return response;

            
        }

        //public async Task UpdatePasswordAsync(string userId, string newPassword)
        //{
        //   Domain.Entities.Identity.AppUser user = await _userManager.FindByIdAsync(userId);

        //    if(user != null)
        //    {
        //        IdentityResult result = await _userManager.ResetPasswordAsync(user, newPassword);
        //    }

        //}
    }
}
