using E_TicaretAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_TicaretAPI.SignalR
{
    public static class HubRegistration
    {
        public static void  MapHubs(this WebApplication webApplication)
        {

            webApplication.MapHub<ProductHub>("/product-hub");

        }
    }
}
