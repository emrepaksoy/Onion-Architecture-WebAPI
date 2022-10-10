using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.Application.Abstraction.Hubs
{
    public interface IProductHubService 
    {
        Task ProductAddedMessageAsync(string message);

    }
}
