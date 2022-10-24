using BooksBazaarAPI.Application.Abstraction.Services;
using BooksBazaarAPI.Application.Repositories;
using BooksBazaarAPI.Domain.Entities.Identity;
using BooksBazaarAPI.Persistance.Contexts;
using BooksBazaarAPI.Persistance.Repositories;
using BooksBazaarAPI.Persistance.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace BooksBazaarAPI.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<BooksBazaarAPIDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;


            }).AddEntityFrameworkStores<BooksBazaarAPIDbContext>();

            services.AddScoped<IBookReadRepository, BookReadRepository>();
            services.AddScoped<IBookWriteRepository, BookWriteRepository>();

            services.AddScoped<IBookImageFileReadRepository, BookImageFileReadRepository>();
            services.AddScoped<IBookImageFileWriteRepository, BookImageFileWriteRepository>();

            services.AddScoped<ICommentReadRepository, CommentReadRepository>();
            services.AddScoped<ICommentWriteRepository, CommentWriteRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICommentService, CommentService>();


        }
    }
}
