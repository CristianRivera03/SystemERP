using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using SystemERP.BLL.Services.Contract;
using SystemERP.BLL.Services.Implementation;
using SystemERP.DAL.DBContext;
using SystemERP.DAL.Repositories.Contract;
using SystemERP.DAL.Repositories.Implementation;
using SystemERP.Utility;

namespace SystemERP.IOC
{
    public static class Dependency
    {
        public static void DependecyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SystemErpDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("connectionDB"));    

            });

            // AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

            // Dependencias de repositorios
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Jwt Settings & Utility
            services.Configure<JwtSettings>(options =>
            {
                options.Key = configuration["JwtSettings:Key"] ?? string.Empty;
                options.Issuer = configuration["JwtSettings:Issuer"] ?? string.Empty;
                options.Audience = configuration["JwtSettings:Audience"] ?? string.Empty;
            });
            services.AddTransient<IJwtUtility, JwtUtility>();

            // Servicios
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
