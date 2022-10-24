using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksBazaarAPI.Application.Abstraction.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string userName, string password);
    }
}
