using BooksBazaarAPI.Application.Abstraction.Services;
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
    public class AuthService : IAuthService
    {
        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        readonly IUserService _userService;
        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
        }

    
        public async Task<bool> LoginAsync(string userName, string password)
        {
            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user,password,false);

            if (result.Succeeded)
                return true;
            else
                return false;
        }
    }
}
