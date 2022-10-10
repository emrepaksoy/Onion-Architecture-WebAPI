
using E_TicaretAPI.Application.Abstraction.Storage;
using E_TicaretAPI.Application.Abstraction.Token;
using E_TicaretAPI.Infrastructure.Enums;
using E_TicaretAPI.Infrastructure.Services;
using E_TicaretAPI.Infrastructure.Services.Storage;
using E_TicaretAPI.Infrastructure.Services.Storage.Azure;
using E_TicaretAPI.Infrastructure.Services.Storage.Local;
using E_TicaretAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace E_TicaretAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastuctureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        } 

        // bu yapı pek tercih edilmez kullanımını görmek  açısından 
        public static void AddStorage<T>(this IServiceCollection serviceCollection, StorageType storageType) 
        {

            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }

        }
    }
}
