using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
        {
            //Thêm DataContext
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            }
            );
            services.AddCors();

            //JWT
           services.AddScoped<ITokenService, TokenService>();

            //Repository (thao tác với database)
            services.AddScoped<IUserRepository, UserRepository>();

            // automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

           return services;
        }
    }
}